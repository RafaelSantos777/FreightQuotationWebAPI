using System.Text.Json;
using System.Text.Json.Nodes;

namespace CargofiveService.Models.DTOs.CargofiveAPI;

public record RateDTO {

    public required string Uuid { get; init; }

    public required string Carrier { get; init; }

    public required List<ScheduleDTO> Schedules { get; init; }

    public static RateDTO FromJsonNode(JsonNode jsonNode) {
        JsonNode productOfferNode = jsonNode["product_offer"]!;
        return new RateDTO {
            Uuid = productOfferNode["rate_uuid"]!.GetValue<string>(),
            Carrier = productOfferNode["main_carrier_name"]!.GetValue<string>(),
            Schedules = jsonNode["schedules"]!.AsArray().Select(node => node.Deserialize<ScheduleDTO>()!).ToList()
        };
    }

}
