using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.WebCargoAPI;

public record RateBreakpointDTO {

    public required string ThresholdString { get; init; }

    public required decimal Cost { get; init; }

    public bool IsBasicCost => ThresholdString == "B";
    public bool IsMinimumCost => ThresholdString == "M";

    public int? GetThresholdValue() {
        if (ThresholdString == "N")
            return 0;
        if (ThresholdString.StartsWith('q'))
            return int.Parse(ThresholdString[1..]);
        return null;
    }

    public bool IsValid() => Cost >= 0;

    public static RateBreakpoint? ToRateBreakpoint(RateBreakpointDTO rateBreakpointDTO) =>
        rateBreakpointDTO.GetThresholdValue() is null ? null : new RateBreakpoint { Threshold = (int)rateBreakpointDTO.GetThresholdValue()!, Cost = rateBreakpointDTO.Cost };

}
