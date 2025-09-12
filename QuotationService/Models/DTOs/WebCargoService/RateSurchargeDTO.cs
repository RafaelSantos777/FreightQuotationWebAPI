namespace QuotationService.Models.DTOs.WebCargoService;

public record RateSurchargeDTO {

    public required string Name { get; init; }

    public required decimal Cost { get; init; }

    public required string CostType { get; init; }

    public required decimal? MinimumCost { get; init; }

    public required decimal? MaximumCost { get; init; }

    public required bool IsMandatory { get; init; }

}
