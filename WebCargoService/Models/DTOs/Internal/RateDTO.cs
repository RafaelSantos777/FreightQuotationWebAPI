using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.Internal;

public record RateDTO {

    public required long UniqueCode { get; init; }

    public required string Airline { get; init; }

    public required string OriginAirportIATACode { get; init; }

    public required string DestinationAirportIATACode { get; init; }

    public required decimal VolumetricFactorMetric { get; init; }

    public required string ProductName { get; init; }

    public required string CurrencyCode { get; init; }

    public required DateOnly ValidFrom { get; init; }

    public required DateOnly? ValidTo { get; init; }

    public required string SpecialHandlingCodeString { get; init; }

    public required List<RateSurchargeDTO> Surcharges { get; init; }

    public required decimal MinimumBreakpointCost { get; init; }

    public required List<RateBreakpointDTO> Breakpoints { get; init; }

    public static RateDTO FromRate(Rate rate) =>
        new() {
            UniqueCode = rate.UniqueCode,
            Airline = rate.Airline,
            OriginAirportIATACode = rate.OriginAirportIATACode,
            DestinationAirportIATACode = rate.DestinationAirportIATACode,
            VolumetricFactorMetric = rate.VolumetricFactorMetric,
            ProductName = rate.ProductName,
            CurrencyCode = rate.CurrencyCode,
            ValidFrom = rate.ValidFrom,
            ValidTo = rate.ValidTo,
            SpecialHandlingCodeString = rate.SpecialHandlingCodeString,
            Surcharges = rate.Surcharges.Select(RateSurchargeDTO.FromRateSurcharge).ToList(),
            MinimumBreakpointCost = rate.MinimumBreakpointCost,
            Breakpoints = rate.Breakpoints.Select(RateBreakpointDTO.FromRateBreakpoint).ToList()
        };

}
