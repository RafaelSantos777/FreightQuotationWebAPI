namespace QuotationService.Services;

public class CargofiveServiceClient(HttpClient httpClient) {

    public async Task<IEnumerable<CargofiveService.RateDTO>> GetAirRates(CargofiveService.RateRequestDTO rateRequestDTO) {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("rates", rateRequestDTO);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CargofiveService.RateDTO>>() ?? [];
    }

}
