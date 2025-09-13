using QuotationService.Models.Entities;

namespace QuotationService.Models.DTOs.Internal;

public record AirQuoteRequestDetailedDTO {

    public required string UserId { get; set; }
    public required LocationDTO OriginAirport { get; init; }

    public required LocationDTO DestinationAirport { get; init; }

    public required double LengthCentimeters { get; init; }

    public required double WidthCentimeters { get; init; }

    public required double HeightCentimeters { get; init; }

    public required double WeightKilograms { get; init; }

    public required string CurrencyCode { get; init; }

    public required SpecialHandlingCodeDTO? SpecialHandlingCode { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }

    public static AirQuoteRequestDetailedDTO FromAirQuoteRequest(AirQuoteRequest airQuoteRequest) =>
        new() {
            UserId = airQuoteRequest.UserId,
            OriginAirport = LocationDTO.FromLocation(airQuoteRequest.OriginAirport),
            DestinationAirport = LocationDTO.FromLocation(airQuoteRequest.DestinationAirport),
            LengthCentimeters = (double)airQuoteRequest.LengthCentimeters,
            WidthCentimeters = (double)airQuoteRequest.WidthCentimeters,
            HeightCentimeters = (double)airQuoteRequest.HeightCentimeters,
            WeightKilograms = (double)airQuoteRequest.WeightKilograms,
            CurrencyCode = airQuoteRequest.CurrencyCode,
            SpecialHandlingCode = airQuoteRequest.SpecialHandlingCode is null ? null : SpecialHandlingCodeDTO.FromSpecialHandlingCode(airQuoteRequest.SpecialHandlingCode),
            CreatedAt = airQuoteRequest.CreatedAt
        };

}
