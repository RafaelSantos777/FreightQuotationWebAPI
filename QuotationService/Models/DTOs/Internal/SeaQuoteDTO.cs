namespace QuotationService.Models.DTOs;

public record SeaQuoteDTO { // TODO

    public required string Carrier { get; init; }

    public required decimal Cost { get; init; }

}
