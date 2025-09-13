namespace QuotationService.Models.DTOs.Internal;

public record SeaQuoteResponseDTO {

    public required long Id { get; init; }

    public required SeaQuoteRequestDetailedDTO SeaQuoteRequestDetailed { get; init; }

    public required List<SeaQuoteDTO> SeaQuotes { get; init; }

}
