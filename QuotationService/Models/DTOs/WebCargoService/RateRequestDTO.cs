namespace QuotationService.Models.DTOs.WebCargoService;

public record RateRequestDTO {

    public required string OriginAirportIATACode { get; init; }

    public required string DestinationAirportIATACode { get; init; }

}
