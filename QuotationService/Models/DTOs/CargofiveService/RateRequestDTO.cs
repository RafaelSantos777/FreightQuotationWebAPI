namespace QuotationService.Models.DTOs.CargofiveService;

public record RateRequestDTO {

    public required string OriginSeaportCargofiveId { get; init; }

    public required string DestinationSeaportCargofiveId { get; init; }

}
