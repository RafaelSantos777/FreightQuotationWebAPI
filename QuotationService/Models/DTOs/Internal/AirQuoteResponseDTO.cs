using QuotationService.Models.Entities;

namespace QuotationService.Models.DTOs;

public record AirQuoteResponseDTO {

    public required long Id { get; init; }

    public required AirQuoteRequestDetailedDTO AirQuoteRequestDetailed { get; init; }

    public required List<AirQuoteDTO> AirQuotes { get; init; }

    public static AirQuoteResponseDTO FromAirQuoteResponse(AirQuoteResponse airQuoteResponse) =>
        new() {
            Id = airQuoteResponse.Id,
            AirQuoteRequestDetailed = AirQuoteRequestDetailedDTO.FromAirQuoteRequest(airQuoteResponse.AirQuoteRequest),
            AirQuotes = airQuoteResponse.AirQuotes.Select(AirQuoteDTO.FromAirQuote).ToList()
        };

}
