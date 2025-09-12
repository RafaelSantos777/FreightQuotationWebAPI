using WebCargoService.Models.Entities;

namespace WebCargoService.Interfaces;

public interface IRateRepository {

    Task<Rate?> GetRateByUniqueCode(long uniqueCode);

    Task AddRateCache(RateCache rateCache);

    Task<RateCache?> GetRateCache(string originAirportIATACode, string destinationAirportIATACode);

    Task UpdateRateCache(RateCache rateCache);

}
