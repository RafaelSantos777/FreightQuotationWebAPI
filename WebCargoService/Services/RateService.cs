using System.Xml.Serialization;
using WebCargoService.Interfaces;
using WebCargoService.Models.DTOs.Internal;
using WebCargoService.Models.Entities;

namespace WebCargoService.Services;

public class RateService(HttpClient httpClient, IRateRepository rateRepository) : IRateService {

    private static readonly XmlSerializer XmlSerializer = new(typeof(List<WebCargoAPI.RateDTO>), new XmlRootAttribute("rates"));

    public async Task<IEnumerable<RateDTO>> GetUpToDateRates(RateRequestDTO rateRequestDTO) {
        string origin = rateRequestDTO.OriginAirportIATACode;
        string destination = rateRequestDTO.DestinationAirportIATACode;
        RateCache? rateCache = await rateRepository.GetRateCache(origin, destination);
        if (rateCache is not null && IsRateCacheUpToDate())
            return rateCache.Rates.Select(RateDTO.FromRate);
        List<Rate> rates = await GetRatesFromWebCargoAPI();
        if (rateCache is null)
            await CreateRateCacheEntity();
        else
            await UpdateRateCacheEntity();
        return rates.Select(RateDTO.FromRate);

        bool IsRateCacheUpToDate() {
            return rateCache.UpdatedAt > DateTime.UtcNow.AddDays(-1);
        }

        async Task<List<Rate>> GetRatesFromWebCargoAPI() {
            string xmlString = await httpClient.GetStringAsync($"sync/rates-v2.php?fullupdate=1&origin={origin}&destination={destination}");
            if (!xmlString.TrimStart().StartsWith("<?xml"))
                return [];
            List<WebCargoAPI.RateDTO> webCargoAPIRateDTOs = (List<WebCargoAPI.RateDTO>?)XmlSerializer.Deserialize(new StringReader(xmlString)) ?? [];
            return webCargoAPIRateDTOs.FindAll(rateDTO => rateDTO.IsValid).Select(WebCargoAPI.RateDTO.ToRate).ToList();
        }

        async Task CreateRateCacheEntity() {
            rateCache = new RateCache {
                OriginAirportIATACode = origin,
                DestinationAirportIATACode = destination,
                Rates = rates,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await rateRepository.AddRateCache(rateCache);
        }

        async Task UpdateRateCacheEntity() {
            HashSet<long> newUniqueCodes = rates.Select(rate => rate.UniqueCode).ToHashSet();
            HashSet<long> cacheUniqueCodes = rateCache.Rates.Select(rate => rate.UniqueCode).ToHashSet();
            List<Rate> newRates = rates.FindAll(rate => !cacheUniqueCodes.Contains(rate.UniqueCode));
            List<Rate> cacheRates = rateCache.Rates.ToList();
            cacheRates.RemoveAll(rate => !newUniqueCodes.Contains(rate.UniqueCode));
            cacheRates.AddRange(newRates);
            RateCache updatedRateCache = new() {
                OriginAirportIATACode = origin,
                DestinationAirportIATACode = destination,
                Rates = cacheRates,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await rateRepository.UpdateRateCache(updatedRateCache);

        }
    }

}
