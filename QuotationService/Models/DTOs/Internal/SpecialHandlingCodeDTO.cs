using QuotationService.Models.Entities;

namespace QuotationService.Models.DTOs.Internal;

public record SpecialHandlingCodeDTO {

    public required string Code { get; init; }

    public required string Description { get; init; }

    public static SpecialHandlingCodeDTO FromSpecialHandlingCode(SpecialHandlingCode specialHandlingCode) => new() { Code = specialHandlingCode.Code, Description = specialHandlingCode.Description };

}
