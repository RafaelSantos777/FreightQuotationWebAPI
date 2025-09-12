using Microsoft.EntityFrameworkCore;
using WebCargoService.Interfaces;
using WebCargoService.Models.Entities;

namespace WebCargoService.Repositories;

public class RateRepository(WebCargoServiceDBContext dbContext) : IRateRepository {

    public async Task<Rate?> GetRateByUniqueCode(long uniqueCode) {
        return await dbContext.Rates.FirstOrDefaultAsync(rate => rate.UniqueCode == uniqueCode);
    }

    public async Task AddRateCache(RateCache rateCache) {
        await dbContext.RateCaches.AddAsync(rateCache);
        await dbContext.SaveChangesAsync();
    }

    public async Task<RateCache?> GetRateCache(string originAirportIATACode, string destinationAirportIATACode) {
        return await GetRateCacheQuery().FirstOrDefaultAsync(cache =>
            cache.OriginAirportIATACode == originAirportIATACode &&
            cache.DestinationAirportIATACode == destinationAirportIATACode);
    }


    public async Task UpdateRateCache(RateCache rateCache) {
        RateCache? existingRateCache = await GetRateCache(rateCache.OriginAirportIATACode, rateCache.DestinationAirportIATACode);
        if (existingRateCache is null)
            throw new InvalidOperationException("RateCache not found for update");
        existingRateCache.UpdatedAt = rateCache.UpdatedAt;
        RemoveObsoleteRates();
        await AddNewRatesToCache();
        await dbContext.SaveChangesAsync();
        return;

        void RemoveObsoleteRates() {
            HashSet<long> newUniqueCodes = rateCache.Rates.Select(rate => rate.UniqueCode).ToHashSet();
            List<Rate> ratesToRemove = existingRateCache.Rates
                .Where(rate => !newUniqueCodes.Contains(rate.UniqueCode))
                .ToList();
            foreach (Rate rate in ratesToRemove)
                existingRateCache.Rates.Remove(rate);
        }

        async Task AddNewRatesToCache() {
            HashSet<long> oldUniqueCodes = existingRateCache.Rates.Select(rate => rate.UniqueCode).ToHashSet();
            List<Rate> ratesToAdd = rateCache.Rates
                .Where(rate => !oldUniqueCodes.Contains(rate.UniqueCode))
                .ToList();
            foreach (Rate rate in ratesToAdd) {
                Rate? existingRate = await GetRateByUniqueCode(rate.UniqueCode);
                existingRateCache.Rates.Add(existingRate ?? rate);
            }
        }
    }

    private IQueryable<RateCache> GetRateCacheQuery() {
        return dbContext.RateCaches.Include(cache => cache.Rates)
            .ThenInclude(rate => rate.Surcharges)
            .Include(cache => cache.Rates)
            .ThenInclude(rate => rate.Breakpoints);
    }

}
