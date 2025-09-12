using Microsoft.EntityFrameworkCore;
using QuotationService.Interfaces;
using QuotationService.Models.Entities;

namespace QuotationService.Repositories;

public class LocationRepository(QuotationServiceDBContext dbContext) : ILocationRepository {

    public async Task<Location?> GetById(long id) => await dbContext.Locations.FindAsync(id);

    public async Task<T?> GetById<T>(long id) where T : Location {
        return await dbContext.Locations
            .OfType<T>()
            .FirstOrDefaultAsync(location => location.Id == id);
    }


    public async Task<IEnumerable<T>> GetAllByType<T>() where T : Location => await dbContext.Locations.OfType<T>().ToListAsync();

    public async Task<IEnumerable<T>> Search<T>(string? name, int page, int limit) where T : Location {
        return await dbContext.Locations
            .OfType<T>()
            .Where(location => string.IsNullOrWhiteSpace(name) || location.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(location => location.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task Add(Location location) {
        await dbContext.Locations.AddAsync(location);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddRange(IEnumerable<Location> locations) {
        await dbContext.Locations.AddRangeAsync(locations);
        await dbContext.SaveChangesAsync();
    }

}
