using System.ComponentModel.DataAnnotations;

namespace QuotationService.Models.Entities;

public class SpecialHandlingCode {

    public long Id { get; set; }

    [StringLength(3)]
    public required string Code { get; set; }

    [StringLength(200)]
    public required string Description { get; set; }

}
