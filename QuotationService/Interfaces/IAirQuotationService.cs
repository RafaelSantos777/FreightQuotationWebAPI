using QuotationService.Exceptions;
using QuotationService.Models.DTOs;

namespace QuotationService.Interfaces;

public interface IAirQuotationService {

    /// <exception cref="InvalidLocationException"></exception>
    /// <exception cref="InvalidSpecialHandlingCodeException"></exception>
    Task<AirQuoteResponseDTO> CreateAirQuoteResponse(AirQuoteRequestDTO airQuoteRequestDTO, string userId);

    /// <exception cref="QuoteNotFoundException"></exception>
    /// <exception cref="ForbiddenAccessException"></exception>
    Task<AirQuoteResponseDTO> GetAirQuoteResponse(long id, string userId);

    /// <exception cref="QuoteNotFoundException"></exception>
    /// <exception cref="ForbiddenAccessException"></exception>
    Task DeleteAirQuoteResponse(long id, string userId);

    Task<IEnumerable<AirQuoteResponseDTO>> GetUserHistory(string userId, int page, int limit);

    Task DeleteUserHistory(string userId);

}
