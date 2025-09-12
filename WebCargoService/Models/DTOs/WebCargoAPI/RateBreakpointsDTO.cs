using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.WebCargoAPI;

public class RateBreakpointsDTO : List<RateBreakpointDTO>, IXmlSerializable {

    public XmlSchema? GetSchema() => null;

    public void ReadXml(XmlReader reader) {
        reader.ReadStartElement("breakpoints");
        while (reader.NodeType != XmlNodeType.EndElement) {
            RateBreakpointDTO rateBreakpoint = new() { ThresholdString = reader.Name, Cost = reader.ReadElementContentAsDecimal() };
            Add(rateBreakpoint);
        }
        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer) {
        throw new NotSupportedException();
    }

    public bool IsValid() {
        return this.All(breakpoint => breakpoint.IsValid())
               && Exists(breakpoint => breakpoint.IsMinimumCost)
               && Exists(breakpoint => breakpoint.GetThresholdValue() == 0);
    }

    public static ICollection<RateBreakpoint> ToRateBreakpointCollection(RateBreakpointsDTO rateBreakpointsDTO) =>
        rateBreakpointsDTO.Select(RateBreakpointDTO.ToRateBreakpoint).OfType<RateBreakpoint>().ToList();

}
