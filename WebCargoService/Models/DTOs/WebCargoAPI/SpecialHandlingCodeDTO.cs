using System.Xml.Serialization;

namespace WebCargoService.Models.DTOs.WebCargoAPI;

public record SpecialHandlingCodeDTO {

    [XmlElement("code")]
    public required string Code { get; init; }

    [XmlElement("description")]
    public required string Description { get; init; }

}
