using System.Diagnostics.CodeAnalysis;

namespace QuotationService.Models.DTOs.WebCargoService;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class RateDTO {

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

}
