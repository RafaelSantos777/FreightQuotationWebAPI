using QuotationService.Interfaces;
using QuotationService.Models.Entities;

namespace QuotationService.Services;

public class SpecialHandlingCodeCache(IServiceProvider serviceProvider) : ISpecialHandlingCodeCache {

    private List<SpecialHandlingCode> _cacheAsList = [];
    private Dictionary<string, SpecialHandlingCode> _cacheAsDictionary = [];

    public async Task InitializeCache() {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        ISpecialHandlingCodeRepository repository = scope.ServiceProvider.GetRequiredService<ISpecialHandlingCodeRepository>();
        _cacheAsList = await repository.GetAll();
        _cacheAsDictionary = _cacheAsList.ToDictionary(specialHandlingCode => specialHandlingCode.Code);
    }

    public IReadOnlyDictionary<string, SpecialHandlingCode> GetCacheAsDictionary() => _cacheAsDictionary;

    public IEnumerable<SpecialHandlingCode> GetCacheAsEnumerable() => _cacheAsList;

}
