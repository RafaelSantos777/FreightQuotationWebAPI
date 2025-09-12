using QuotationService.Models.Entities;

namespace QuotationService.Models.DTOs.Internal;

public record LocationDTO {

    public required long Id { get; init; }

    public required string Name { get; init; }

    public required string CountryCode { get; init; }

    public required double? Latitude { get; init; }

    public required double? Longitude { get; init; }

    public required string Type { get; init; }

    public static LocationDTO FromLocation(Location location) =>
        new() {
            Id = location.Id,
            Name = location.Name,
            CountryCode = location.CountryCode,
            Latitude = (double?)location.Latitude,
            Longitude = (double?)location.Longitude,
            Type = location.GetType().Name
        };

}
