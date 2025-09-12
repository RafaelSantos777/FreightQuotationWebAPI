namespace CargofiveService.Models.DTOs.Internal;

public record ScheduleDTO {

    public required DateTimeOffset DepartureDate { get; init; }

    public required DateTimeOffset ArrivalDate { get; init; }

}
