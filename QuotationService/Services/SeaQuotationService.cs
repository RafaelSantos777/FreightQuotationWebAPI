using QuotationService.Interfaces;
using QuotationService.Models.DTOs.Internal;

namespace QuotationService.Services;

public class SeaQuotationService : ISeaQuotationService {

    public Task<SeaQuoteResponseDTO> CreateSeaQuoteResponse(SeaQuoteRequestDTO seaQuoteRequestDTO, string userId) => throw new NotImplementedException();

    public Task<SeaQuoteResponseDTO> GetSeaQuoteResponse(long id, string userId) => throw new NotImplementedException();

    public Task DeleteSeaQuoteResponse(long id, string userId) => throw new NotImplementedException();

    public Task<IEnumerable<SeaQuoteResponseDTO>> GetUserHistory(string userId, int page, int limit) => throw new NotImplementedException();

    public Task DeleteUserHistory(string userId) {
        Console.WriteLine("DeleteUserHistory() for SeaQuotationService was called, but is not implemented yet");
        return Task.CompletedTask;
    }

}
