namespace WebCargoService.Models.DTOs.Internal;

public record RateRequestDTO {

    public required string OriginAirportIATACode { get; init; }

    public required string DestinationAirportIATACode { get; init; }

}
