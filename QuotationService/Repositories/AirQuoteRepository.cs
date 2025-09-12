using Microsoft.EntityFrameworkCore;
using QuotationService.Interfaces;
using QuotationService.Models.Entities;

namespace QuotationService.Repositories;

public class AirQuoteRepository(QuotationServiceDBContext dbContext) : IAirQuoteRepository {

    public async Task AddAirQuoteResponse(AirQuoteResponse airQuoteResponse) {
        AirQuoteRequest airQuoteRequest = airQuoteResponse.AirQuoteRequest;
        dbContext.Attach(airQuoteRequest.OriginAirport);
        dbContext.Attach(airQuoteRequest.DestinationAirport);
        if (airQuoteRequest.SpecialHandlingCode is not null)
            dbContext.Attach(airQuoteRequest.SpecialHandlingCode);
        foreach (AirQuote airQuote in airQuoteResponse.AirQuotes)
            dbContext.Attach(airQuote.SpecialHandlingCode);
        await dbContext.AirQuoteResponses.AddAsync(airQuoteResponse);
        await dbContext.SaveChangesAsync();
    }

    public async Task<AirQuoteResponse?> GetAirQuoteResponseById(long id) {
        return await GetAirQuoteResponseQuery().FirstOrDefaultAsync(response => response.Id == id);
    }

    public async Task<IEnumerable<AirQuoteResponse>> GetUserHistory(string userId, int page, int limit) {
        return await GetAirQuoteResponseQuery()
            .Where(response => response.AirQuoteRequest.UserId == userId)
            .OrderByDescending(response => response.AirQuoteRequest.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task DeleteAirQuoteResponse(AirQuoteResponse airQuoteResponse) {
        dbContext.AirQuoteResponses.Remove(airQuoteResponse);
        dbContext.AirQuoteRequests.Remove(airQuoteResponse.AirQuoteRequest);
        dbContext.AirQuotes.RemoveRange(airQuoteResponse.AirQuotes);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserHistory(string userId) {
        List<AirQuoteResponse> userAirQuoteResponses = await GetAirQuoteResponseQuery()
            .Where(response => response.AirQuoteRequest.UserId == userId)
            .ToListAsync();
        foreach (AirQuoteResponse userAirQuoteResponse in userAirQuoteResponses)
            await DeleteAirQuoteResponse(userAirQuoteResponse);
    }

    private IQueryable<AirQuoteResponse> GetAirQuoteResponseQuery() {
        return dbContext.AirQuoteResponses
            .Include(response => response.AirQuoteRequest)
            .Include(response => response.AirQuoteRequest.OriginAirport)
            .Include(response => response.AirQuoteRequest.DestinationAirport)
            .Include(response => response.AirQuotes)
            .ThenInclude(quote => quote.SpecialHandlingCode);
    }

}
