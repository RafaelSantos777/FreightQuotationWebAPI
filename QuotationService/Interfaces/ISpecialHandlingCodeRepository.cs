using QuotationService.Models.Entities;

namespace QuotationService.Interfaces;

public interface ISpecialHandlingCodeRepository {

    Task<List<SpecialHandlingCode>> GetAll();

    Task AddRange(IEnumerable<SpecialHandlingCode> specialHandlingCodes);

}
