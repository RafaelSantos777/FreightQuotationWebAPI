using System.Text.Json.Serialization;

namespace CargofiveService.Models.DTOs.CargofiveAPI;

public record ScheduleDTO {

    [JsonPropertyName("departure_date")]
    public required DateTimeOffset DepartureDate { get; init; }

    [JsonPropertyName("arrival_date")]
    public required DateTimeOffset ArrivalDate { get; init; }

}
