using CargofiveService.Interfaces;
using CargofiveService.Models.DTOs.Internal;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CargofiveService.Services;

public class SeaportService(HttpClient httpClient) : ISeaportService {

    public async Task<IEnumerable<SeaportDTO>> GetSeaports(int page, int limit) {
        JsonNode jsonNode = (await httpClient.GetFromJsonAsync<JsonNode>($"api/v1/public/places?place_type_id={CargofiveAPI.SeaportDTO.SeaportLocationTypeId}&page={page}&limit={limit}"))!;
        List<CargofiveAPI.SeaportDTO> cargoFiveAPISeaportDTOs = jsonNode["data"].Deserialize<List<CargofiveAPI.SeaportDTO>>()!.FindAll(seaportDTO => seaportDTO.IsSeaport);
        return cargoFiveAPISeaportDTOs.Select(SeaportDTO.FromCargofiveAPISeaportDTO);
    }

}
