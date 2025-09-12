namespace QuotationService.Models.DTOs.CargofiveService;

public record ScheduleDTO {

    public required DateTimeOffset DepartureDate { get; init; }

    public required DateTimeOffset ArrivalDate { get; init; }

}
