using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebCargoService.Models.Enums;

namespace WebCargoService.Models.Entities;

public class RateSurcharge {

    public long Id { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    [Precision(10, 2)]
    public required decimal Cost { get; set; }

    public required RateSurchargeCostType CostType { get; set; }

    [Precision(10, 2)]
    public required decimal? MinimumCost { get; set; }

    [Precision(10, 2)]
    public required decimal? MaximumCost { get; set; }

    public required bool IsMandatory { get; set; }

}
