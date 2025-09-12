using WebCargoService.Models.Entities;
using WebCargoService.Models.Enums;
using System.Xml.Serialization;

namespace WebCargoService.Models.DTOs.WebCargoAPI;

[XmlType("surcharge")]
public record RateSurchargeDTO {

    [XmlElement("name")]
    public required string Name { get; init; }

    [XmlElement("cost")]
    public required decimal Cost { get; init; }

    [XmlElement("type")]
    public required string CostType { get; init; }

    [XmlElement("min")]
    public required string MinimumCostString { get; init; }

    [XmlElement("max")]
    public required string MaximumCostString { get; init; }

    [XmlElement("mandatory")]
    public required bool IsMandatory { get; init; }

    [XmlIgnore]
    private decimal? MinimumCost => string.IsNullOrWhiteSpace(MinimumCostString) ? null : decimal.Parse(MinimumCostString);

    [XmlIgnore]
    private decimal? MaximumCost => string.IsNullOrWhiteSpace(MaximumCostString) ? null : decimal.Parse(MaximumCostString);

    [XmlIgnore]
    private bool IsIncluded => Cost <= 0;

    [XmlIgnore]
    public bool IsValid => CostType is "fix" or "kg" or "pkg";

    public static RateSurcharge ToRateSurcharge(RateSurchargeDTO rateSurchargeDTO) {
        RateSurchargeCostType costType = rateSurchargeDTO.CostType switch {
            "fix" => RateSurchargeCostType.Fix,
            "kg" => RateSurchargeCostType.Kilograms,
            "pkg" => RateSurchargeCostType.Package,
            _ => throw new ArgumentException("Invalid cost type", nameof(rateSurchargeDTO))
        };
        return new RateSurcharge {
            Name = rateSurchargeDTO.Name,
            Cost = rateSurchargeDTO.IsIncluded ? 0 : rateSurchargeDTO.Cost,
            CostType = costType,
            MinimumCost = rateSurchargeDTO.MinimumCost,
            MaximumCost = rateSurchargeDTO.MaximumCost,
            IsMandatory = rateSurchargeDTO.IsMandatory
        };
    }

}
