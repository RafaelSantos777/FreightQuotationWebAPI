using Microsoft.EntityFrameworkCore;
using UserService.Interfaces;
using UserService.Models.Entities;

namespace UserService.Repositories;

public class DeltaLinkRepository(UserServiceDBContext dbContext) : IDeltaLinkRepository {

    public async Task<DeltaLink?> Get() => await dbContext.DeltaLinks.FirstOrDefaultAsync();

    public async Task AddOrUpdate(DeltaLink deltaLink) {
        DeltaLink? existingDeltaLink = await Get();
        if (existingDeltaLink is null)
            dbContext.DeltaLinks.Add(deltaLink);
        else
            existingDeltaLink.Link = deltaLink.Link;
        await dbContext.SaveChangesAsync();
    }

}
