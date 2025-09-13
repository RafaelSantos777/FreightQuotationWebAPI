using QuotationService.Models.DTOs.Internal;

namespace QuotationService.Interfaces;

public interface ILocationService {

    Task<IEnumerable<LocationDTO>> Search(string? search, string? type, int page, int limit);

    Task<LocationDTO?> GetById(long id);

}
