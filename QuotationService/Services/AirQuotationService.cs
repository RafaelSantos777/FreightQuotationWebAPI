using QuotationService.Exceptions;
using QuotationService.Interfaces;
using QuotationService.Models.DTOs.Internal;
using QuotationService.Models.Entities;

namespace QuotationService.Services;

public class AirQuotationService(
    WebCargoServiceClient webCargoServiceClient,
    ISpecialHandlingCodeCache specialHandlingCodeCache,
    IAirQuoteRepository airQuoteRepository,
    ILocationRepository locationRepository
) : IAirQuotationService {

    public async Task<AirQuoteResponseDTO> CreateAirQuoteResponse(AirQuoteRequestDTO airQuoteRequestDTO, string userId) {
        IReadOnlyDictionary<string, SpecialHandlingCode> specialHandlingCodeDictionary = specialHandlingCodeCache.GetCacheAsDictionary();
        (Airport originAirport, Airport destinationAirport) = await GetAirports();
        ValidateRequestSpecialHandlingCode();
        List<WebCargoService.RateDTO> rateDTOs = await RetrieveRateDTOs();
        rateDTOs = FilterAndConvertValidRates();
        AirQuoteRequest airQuoteRequest = CreateAirQuoteRequest();
        List<AirQuote> quotes = CalculateQuotes();
        AirQuoteResponse airQuoteResponse = new() { AirQuoteRequest = airQuoteRequest, AirQuotes = quotes };
        await airQuoteRepository.AddAirQuoteResponse(airQuoteResponse);
        return AirQuoteResponseDTO.FromAirQuoteResponse(airQuoteResponse);

        async Task<(Airport originAirport, Airport destinationAirport)> GetAirports() {
            Airport origin =
                await locationRepository.GetById<Airport>(airQuoteRequestDTO.OriginAirportId) ??
                throw new InvalidLocationException("Origin location not found or not an airport");
            Airport destination =
                await locationRepository.GetById<Airport>(airQuoteRequestDTO.DestinationAirportId) ??
                throw new InvalidLocationException("Destination location not found or not an airport");
            return (origin, destination);
        }

        void ValidateRequestSpecialHandlingCode() {
            if (airQuoteRequestDTO.SpecialHandlingCode is not null && !specialHandlingCodeDictionary.ContainsKey(airQuoteRequestDTO.SpecialHandlingCode))
                throw new InvalidSpecialHandlingCodeException("Invalid special handling code");
        }

        async Task<List<WebCargoService.RateDTO>> RetrieveRateDTOs() {
            WebCargoService.RateRequestDTO rateRequestDTO = new() {
                OriginAirportIATACode = originAirport.IATACode,
                DestinationAirportIATACode = destinationAirport.IATACode
            };
            return (await webCargoServiceClient.GetAirRates(rateRequestDTO)).ToList();
        }

        List<WebCargoService.RateDTO> FilterAndConvertValidRates() {

            return rateDTOs
                .Where(rateDTO =>
                    (airQuoteRequestDTO.SpecialHandlingCode is null || rateDTO.SpecialHandlingCodeString == airQuoteRequestDTO.SpecialHandlingCode)
                    && specialHandlingCodeDictionary.ContainsKey(rateDTO.SpecialHandlingCodeString))
                .ToList();
        }

        AirQuoteRequest CreateAirQuoteRequest() {
            return AirQuoteRequestDTO.ToAirQuoteRequest(
                airQuoteRequestDTO,
                originAirport,
                destinationAirport,
                airQuoteRequestDTO.SpecialHandlingCode is null ? null : specialHandlingCodeDictionary[airQuoteRequestDTO.SpecialHandlingCode], userId);
        }

        List<AirQuote> CalculateQuotes() {
            return rateDTOs.Select(rateDTO => new AirQuote {
                Airline = rateDTO.Airline,
                ProductName = rateDTO.ProductName,
                SpecialHandlingCode = specialHandlingCodeDictionary[rateDTO.SpecialHandlingCodeString],
                Cost = Math.Round(rateDTO.Surcharges.Select(CalculateSurchargeCost).Sum() + CalculateBreakpointsCost(rateDTO), 2, MidpointRounding.ToPositiveInfinity)
            }).ToList();

            decimal CalculateSurchargeCost(WebCargoService.RateSurchargeDTO surcharge) {
                if (!surcharge.IsMandatory)
                    return 0;
                decimal cost = surcharge.CostType.ToLower() switch {
                    "fix" => surcharge.Cost,
                    "kilograms" => surcharge.Cost * airQuoteRequest.WeightKilograms,
                    "package" => surcharge.Cost * airQuoteRequest.WeightKilograms, // TODO Package Cost Type
                    _ => throw new ArgumentOutOfRangeException(nameof(surcharge))
                };
                return Math.Min(Math.Max(cost, surcharge.MinimumCost ?? 0), surcharge.MaximumCost ?? decimal.MaxValue);
            }

            decimal CalculateBreakpointsCost(WebCargoService.RateDTO rateDTO) {
                double volumeCubicCentimeters = (double)(airQuoteRequest.LengthCentimeters * airQuoteRequest.WidthCentimeters * airQuoteRequest.HeightCentimeters);
                double dimensionalWeightMetric = Math.Max((double)airQuoteRequest.WeightKilograms, volumeCubicCentimeters / (double)rateDTO.VolumetricFactorMetric);
                WebCargoService.RateBreakpointDTO[] orderedBreakpoints = rateDTO.Breakpoints.OrderBy(breakpoint => breakpoint.Threshold).ToArray();
                WebCargoService.RateBreakpointDTO applicableBreakpoint = orderedBreakpoints.Last(breakpoint => breakpoint.Threshold <= dimensionalWeightMetric);
                return Math.Max(rateDTO.MinimumBreakpointCost, applicableBreakpoint.Cost * (decimal)dimensionalWeightMetric);
            }
        }

    }

    public async Task<AirQuoteResponseDTO> GetAirQuoteResponse(long id, string userId) {
        AirQuoteResponse? airQuoteResponse = await airQuoteRepository.GetAirQuoteResponseById(id);
        if (airQuoteResponse is null)
            throw new QuoteNotFoundException("Air quote not found");
        if (airQuoteResponse.AirQuoteRequest.UserId != userId)
            throw new ForbiddenAccessException("You are not allowed to get this quote");
        return AirQuoteResponseDTO.FromAirQuoteResponse(airQuoteResponse);
    }

    public async Task DeleteAirQuoteResponse(long id, string userId) {
        AirQuoteResponse? airQuoteResponse = await airQuoteRepository.GetAirQuoteResponseById(id);
        if (airQuoteResponse is null)
            throw new QuoteNotFoundException("Air quote not found");
        if (airQuoteResponse.AirQuoteRequest.UserId != userId)
            throw new ForbiddenAccessException("You are not allowed to delete this quote");
        await airQuoteRepository.DeleteAirQuoteResponse(airQuoteResponse);
    }

    public async Task<IEnumerable<AirQuoteResponseDTO>> GetUserHistory(string userId, int page, int limit) {
        IEnumerable<AirQuoteResponse> airQuoteResponses = await airQuoteRepository.GetUserHistory(userId, page, limit);
        return airQuoteResponses.Select(AirQuoteResponseDTO.FromAirQuoteResponse);
    }

    public async Task DeleteUserHistory(string userId) {
        await airQuoteRepository.DeleteUserHistory(userId);
    }

}
