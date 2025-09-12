namespace QuotationService.Models.DTOs.Internal;

public record SeaQuoteDTO { // TODO

    public required string Carrier { get; init; }

    public required decimal Cost { get; init; }

}
