using QuotationService.Exceptions;
using QuotationService.Models.DTOs;

namespace QuotationService.Interfaces;

public interface ISeaQuotationService {

    /// <exception cref="InvalidLocationException"></exception>
    /// <exception cref="InvalidSpecialHandlingCodeException"></exception>
    Task<SeaQuoteResponseDTO> CreateSeaQuoteResponse(SeaQuoteRequestDTO seaQuoteRequestDTO, string userId);

    /// <exception cref="QuoteNotFoundException"></exception>
    /// <exception cref="ForbiddenAccessException"></exception>
    Task<SeaQuoteResponseDTO> GetSeaQuoteResponse(long id, string userId);

    /// <exception cref="QuoteNotFoundException"></exception>
    /// <exception cref="ForbiddenAccessException"></exception>
    Task DeleteSeaQuoteResponse(long id, string userId);

    Task<IEnumerable<SeaQuoteResponseDTO>> GetUserHistory(string userId, int page, int limit);

    Task DeleteUserHistory(string userId);

}
