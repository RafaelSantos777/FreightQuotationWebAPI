using QuotationService.Models.Entities;

namespace QuotationService.Models.DTOs.Internal;

public record AirQuoteDTO {

    public required string Airline { get; init; }

    public required string ProductName { get; init; }

    public required SpecialHandlingCodeDTO SpecialHandlingCode { get; init; }

    public required decimal Cost { get; init; }

    public static AirQuoteDTO FromAirQuote(AirQuote airQuote) =>
        new() {
            Airline = airQuote.Airline,
            ProductName = airQuote.ProductName,
            SpecialHandlingCode = SpecialHandlingCodeDTO.FromSpecialHandlingCode(airQuote.SpecialHandlingCode),
            Cost = airQuote.Cost
        };

}
