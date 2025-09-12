using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.Internal;

public record RateBreakpointDTO {

    public required int Threshold { get; init; }

    public required decimal Cost { get; init; }


    public static RateBreakpointDTO FromRateBreakpoint(RateBreakpoint rateBreakpoint) =>
        new() {
            Threshold = rateBreakpoint.Threshold,
            Cost = rateBreakpoint.Cost
        };

}
