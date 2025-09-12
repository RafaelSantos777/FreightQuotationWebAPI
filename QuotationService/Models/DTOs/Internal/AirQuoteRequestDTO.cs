using QuotationService.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.DTOs.Internal;
public record AirQuoteRequestDTO : IValidatableObject {

    [Required]
    public required long OriginAirportId { get; init; }
    [Required]
    public required long DestinationAirportId { get; init; }
    [Required]
    public required double LengthCentimeters { get; init; }
    [Required]
    public required double WidthCentimeters { get; init; }
    [Required]
    public required double HeightCentimeters { get; init; }
    [Required]
    public required double WeightKilograms { get; init; }
    
    public required string CurrencyCode { get; init; }
    
    public string? SpecialHandlingCode { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (OriginAirportId == DestinationAirportId)
            yield return new ValidationResult("Origin and destination airports must be different");
        if (LengthCentimeters <= 0)
            yield return new ValidationResult("Length must be greater than 0");
        if (WidthCentimeters <= 0)
            yield return new ValidationResult("Width must be greater than 0");
        if (HeightCentimeters <= 0)
            yield return new ValidationResult("Height must be greater than 0");
        if (WeightKilograms <= 0)
            yield return new ValidationResult("Weight must be greater than 0");
    }

    public static AirQuoteRequest ToAirQuoteRequest(AirQuoteRequestDTO airQuoteRequestDTO, Airport originAirport, Airport destinationAirport, SpecialHandlingCode? specialHandlingCode, string userId) {
        if (originAirport.Id != airQuoteRequestDTO.OriginAirportId || destinationAirport.Id != airQuoteRequestDTO.DestinationAirportId)
            throw new ArgumentException("Airports do not match", nameof(airQuoteRequestDTO));
        return new AirQuoteRequest {
            UserId = userId,
            OriginAirport = originAirport,
            DestinationAirport = destinationAirport,
            LengthCentimeters = (decimal)airQuoteRequestDTO.LengthCentimeters,
            WidthCentimeters = (decimal)airQuoteRequestDTO.WidthCentimeters,
            HeightCentimeters = (decimal)airQuoteRequestDTO.HeightCentimeters,
            WeightKilograms = (decimal)airQuoteRequestDTO.WeightKilograms,
            CurrencyCode = airQuoteRequestDTO.CurrencyCode,
            SpecialHandlingCode = specialHandlingCode,
            CreatedAt = DateTimeOffset.Now
        };
    }

}
