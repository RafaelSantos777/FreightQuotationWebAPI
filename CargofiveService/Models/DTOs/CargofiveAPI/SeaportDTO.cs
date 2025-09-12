using System.Text.Json.Serialization;

namespace CargofiveService.Models.DTOs.CargofiveAPI;

public record SeaportDTO {

    [JsonPropertyName("id")]
    public required long Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonPropertyName("coordinates")]
    public required string? Coordinates { get; init; }

    public const int SeaportLocationTypeId = 1;
    public bool IsSeaport => Id > 0;

    public string CountryCode => Code[..2];

    public (double latitude, double longitude)? GetCoordinatesInDoubles() {
        if (Coordinates is null)
            return null;
        string[] coordinates = Coordinates.Split(' ');
        if (coordinates.Length != 2)
            return null;
        double? latitude = TryParseCoordinate(coordinates[0]);
        double? longitude = TryParseCoordinate(coordinates[1]);
        if (latitude is null || longitude is null)
            return null;
        return (latitude.Value, longitude.Value);

        double? TryParseCoordinate(string coordinate) {
            return double.TryParse(coordinate, out double result) ? result : null;
        }
    }

}
