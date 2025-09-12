namespace QuotationService.Models.DTOs.WebCargoService;

public record RateBreakpointDTO {

    public required int Threshold { get; init; }

    public required decimal Cost { get; init; }

}
