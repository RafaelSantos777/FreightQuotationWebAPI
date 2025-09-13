namespace QuotationService.Models.DTOs.Internal;

public record SeaQuoteRequestDetailedDTO {

    public required string UserId { get; set; }

    public required LocationDTO OriginSeaport { get; init; }

    public required LocationDTO DestinationSeaport { get; init; }

    public required string CurrencyCode { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }

}
