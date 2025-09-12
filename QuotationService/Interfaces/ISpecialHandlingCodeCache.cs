using QuotationService.Models.Entities;

namespace QuotationService.Interfaces;

public interface ISpecialHandlingCodeCache {

    Task InitializeCache();
    IEnumerable<SpecialHandlingCode> GetCacheAsEnumerable();
    IReadOnlyDictionary<string, SpecialHandlingCode> GetCacheAsDictionary();

}
