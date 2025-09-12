using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.Entities;

public abstract class Location {

    public long Id { get; set; }

    [StringLength(200)]
    public required string Name { get; set; }

    [StringLength(2)]
    public required string CountryCode { get; set; }

    [Precision(9, 6)]
    public required decimal? Latitude { get; set; }

    [Precision(9, 6)]
    public required decimal? Longitude { get; set; }

}

public class Airport : Location {

    [StringLength(3)]
    public required string IATACode { get; set; }

}

public class Seaport : Location {

    public required long CargofiveLocationId { get; set; }

}
