namespace CargofiveService.Models.DTOs.Internal;

public record RateRequestDTO {

    public required string OriginSeaportId { get; init; }

    public required string DestinationSeaportId { get; init; }

}
