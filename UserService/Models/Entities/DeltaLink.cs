using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities;

public class DeltaLink {

    public long Id { get; set; }

    [StringLength(int.MaxValue)]
    public required string Link { get; set; }

}
