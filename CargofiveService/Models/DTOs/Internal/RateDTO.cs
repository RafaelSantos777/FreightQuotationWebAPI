namespace CargofiveService.Models.DTOs.Internal;

public record RateDTO {

    public required string UUid { get; init; }

    public required string Carrier { get; init; }

    public required List<ScheduleDTO> Schedules { get; init; }

}
