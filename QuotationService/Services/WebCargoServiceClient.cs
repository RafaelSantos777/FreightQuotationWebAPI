namespace QuotationService.Services;

public class WebCargoServiceClient(HttpClient httpClient) {

    public async Task<IEnumerable<WebCargoService.RateDTO>> GetAirRates(WebCargoService.RateRequestDTO rateRequestDTO) {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("rates", rateRequestDTO);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<WebCargoService.RateDTO>>() ?? [];
    }

}
