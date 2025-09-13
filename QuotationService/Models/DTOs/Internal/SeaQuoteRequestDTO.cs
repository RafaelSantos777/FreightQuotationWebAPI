using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.DTOs.Internal;

public record SeaQuoteRequestDTO { // TODO

    [Required]
    public required long OriginSeaportId { get; init; }

    [Required]
    public required long DestinationSeaportId { get; init; }


    public required string CurrencyCode { get; init; }

}
