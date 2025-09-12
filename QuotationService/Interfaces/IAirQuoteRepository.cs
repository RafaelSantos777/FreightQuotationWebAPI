using QuotationService.Models.Entities;

namespace QuotationService.Interfaces;

public interface IAirQuoteRepository {

    Task AddAirQuoteResponse(AirQuoteResponse airQuoteResponse);

    Task<AirQuoteResponse?> GetAirQuoteResponseById(long id);

    Task<IEnumerable<AirQuoteResponse>> GetUserHistory(string userId, int page, int limit);

    Task DeleteAirQuoteResponse(AirQuoteResponse airQuoteResponse);

    Task DeleteUserHistory(string userId);

}
