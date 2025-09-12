using Microsoft.EntityFrameworkCore;

namespace WebCargoService.Models.Entities;

public class RateBreakpoint {

    public long Id { get; set; }

    public required int Threshold { get; set; }

    [Precision(10, 2)]
    public required decimal Cost { get; set; }

}
