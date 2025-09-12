using System.ComponentModel.DataAnnotations;

namespace WebCargoService.Models.Entities;

public class RateCache { // TODO Use Redis Cache instead

    [StringLength(3)]
    public required string OriginAirportIATACode { get; set; }

    [StringLength(3)]
    public required string DestinationAirportIATACode { get; set; }

    public required ICollection<Rate> Rates { get; set; }

    public required DateTimeOffset UpdatedAt { get; set; }

}
