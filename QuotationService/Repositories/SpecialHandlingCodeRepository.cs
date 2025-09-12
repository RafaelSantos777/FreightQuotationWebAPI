using Microsoft.EntityFrameworkCore;
using QuotationService.Interfaces;
using QuotationService.Models.Entities;

namespace QuotationService.Repositories;

public class SpecialHandlingCodeRepository(QuotationServiceDBContext dbContext) : ISpecialHandlingCodeRepository {

    public async Task<List<SpecialHandlingCode>> GetAll() {
        return await dbContext.SpecialHandlingCodes
            .OrderBy(specialHandlingCode => specialHandlingCode.Code)
            .ToListAsync();
    }

    public async Task AddRange(IEnumerable<SpecialHandlingCode> specialHandlingCodes) {
        await dbContext.SpecialHandlingCodes.AddRangeAsync(specialHandlingCodes);
        await dbContext.SaveChangesAsync();
    }

}
