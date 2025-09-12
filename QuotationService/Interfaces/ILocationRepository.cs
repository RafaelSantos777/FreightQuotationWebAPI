using QuotationService.Models.Entities;

namespace QuotationService.Interfaces;

public interface ILocationRepository {

    Task<Location?> GetById(long id);

    Task<T?> GetById<T>(long id) where T : Location;

    Task<IEnumerable<T>> GetAllByType<T>() where T : Location;

    Task<IEnumerable<T>> Search<T>(string? name, int page, int limit) where T : Location;

    Task Add(Location location);

    Task AddRange(IEnumerable<Location> locations);

}
