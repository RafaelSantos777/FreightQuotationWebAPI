using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebCargoService.Models.Entities;

public class Rate {

    public long Id { get; set; }

    public required long UniqueCode { get; set; }

    [StringLength(200)]
    public required string Airline { get; set; }

    [StringLength(3)]
    public required string OriginAirportIATACode { get; set; }

    [StringLength(3)]
    public required string DestinationAirportIATACode { get; set; }

    [Precision(10, 2)]
    public required decimal VolumetricFactorMetric { get; set; }

    [StringLength(200)]
    public required string ProductName { get; set; }

    [StringLength(3)]
    public required string CurrencyCode { get; set; }

    public required DateOnly ValidFrom { get; set; }

    public required DateOnly? ValidTo { get; set; }

    [StringLength(3)]
    public required string SpecialHandlingCodeString { get; set; }

    public required ICollection<RateSurcharge> Surcharges { get; set; }

    [Precision(10, 2)]
    public required decimal MinimumBreakpointCost { get; set; }

    public required ICollection<RateBreakpoint> Breakpoints { get; set; }

}
