using CargofiveService.Interfaces;
using CargofiveService.Models.DTOs.Internal;
using System.Text.Json.Nodes;

namespace CargofiveService.Services;

public class RateService(HttpClient httpClient) : IRateService {

    public async Task<IEnumerable<RateDTO>> GetUpToDateRates(RateRequestDTO rateRequestDTO) {

        JsonNode jsonNode = (await httpClient.GetFromJsonAsync<JsonNode>(CreateQuery()))!;
        JsonArray ratesJSONArray = jsonNode["offers"]!["rates"]!.AsArray();
        List<CargofiveAPI.RateDTO> cargofiveAPIRateDTOs = ratesJSONArray.Select(CargofiveAPI.RateDTO.FromJsonNode!).ToList();
        foreach (CargofiveAPI.RateDTO cargofiveAPIRateDTO in cargofiveAPIRateDTOs)
            Console.WriteLine(cargofiveAPIRateDTO);
        return []; // TODO

        string CreateQuery() {
            return "api/v1/public/rates" +
                   "?providers=-1" +
                   "&api_providers=-1" +
                   $"&origins={rateRequestDTO.OriginSeaportId}" +
                   $"&destinations={rateRequestDTO.DestinationSeaportId}" +
                   "&type=FCL" +
                   "&client_id=" +
                   "&contact_id=" +
                   "&search_id=" +
                   "&integrations=true" +
                   "&include_destination_charges=true" +
                   "&include_origin_charges=true" +
                   "&include_imo_charges=false" +
                   "&cargo_details=1x20DVx0,2x40DVx0" +
                   "&departure_date=2025-06-10";
        }

    }

}
