namespace CargofiveService.Models.DTOs.Internal;

public record SeaportDTO {

    public required long Id { get; init; }

    public required string Name { get; init; }

    public required string CountryCode { get; init; }

    public required double? Latitude { get; init; }

    public required double? Longitude { get; init; }


    public static SeaportDTO FromCargofiveAPISeaportDTO(CargofiveAPI.SeaportDTO seaportDTO) {
        (double latitude, double longitude)? coordinatesInDoubles = seaportDTO.GetCoordinatesInDoubles();
        return new SeaportDTO {
            Id = seaportDTO.Id,
            Name = seaportDTO.Name,
            CountryCode = seaportDTO.CountryCode,
            Latitude = coordinatesInDoubles?.latitude,
            Longitude = coordinatesInDoubles?.longitude
        };
    }

}
