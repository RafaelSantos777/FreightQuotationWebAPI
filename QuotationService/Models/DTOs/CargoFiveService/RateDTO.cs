namespace QuotationService.Models.DTOs.CargofiveService;

public class RateDTO {

    public required string UUid { get; init; }

    public required string Carrier { get; init; }

    public required List<ScheduleDTO> Schedules { get; init; }

}
