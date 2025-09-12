using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.Internal;

public record RateSurchargeDTO {

    public required string Name { get; init; }

    public required decimal Cost { get; init; }

    public required string CostType { get; init; }

    public required decimal? MinimumCost { get; init; }

    public required decimal? MaximumCost { get; init; }

    public required bool IsMandatory { get; init; }

    public static RateSurchargeDTO FromRateSurcharge(RateSurcharge rateSurcharge) =>
        new() {
            Name = rateSurcharge.Name,
            Cost = rateSurcharge.Cost,
            CostType = rateSurcharge.CostType.ToString(),
            MinimumCost = rateSurcharge.MinimumCost,
            MaximumCost = rateSurcharge.MaximumCost,
            IsMandatory = rateSurcharge.IsMandatory
        };

}
