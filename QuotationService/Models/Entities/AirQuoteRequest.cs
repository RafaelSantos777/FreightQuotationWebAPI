using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.Entities;

public class AirQuoteRequest {

    public long Id { get; set; }

    [StringLength(100)]
    public required string UserId { get; set; }

    public required Airport OriginAirport { get; set; }

    public required Airport DestinationAirport { get; set; }

    [Precision(10, 2)]
    public required decimal LengthCentimeters { get; set; }

    [Precision(10, 2)]
    public required decimal WidthCentimeters { get; set; }

    [Precision(10, 2)]
    public required decimal HeightCentimeters { get; set; }

    [Precision(10, 2)]
    public required decimal WeightKilograms { get; set; }

    [StringLength(3)]
    public required string CurrencyCode { get; set; }

    public required SpecialHandlingCode? SpecialHandlingCode { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

}
