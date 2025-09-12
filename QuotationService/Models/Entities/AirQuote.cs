using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.Entities;

public class AirQuote {

    public long Id { get; set; }

    [StringLength(200)]
    public required string Airline { get; set; }

    [StringLength(200)]
    public required string ProductName { get; set; }

    public required SpecialHandlingCode SpecialHandlingCode { get; set; }

    [Precision(10, 2)]
    public required decimal Cost { get; set; }

}
