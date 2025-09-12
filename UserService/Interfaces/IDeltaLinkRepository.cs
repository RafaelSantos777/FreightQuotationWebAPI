using UserService.Models.Entities;

namespace UserService.Interfaces;

public interface IDeltaLinkRepository {

    Task<DeltaLink?> Get();

    Task AddOrUpdate(DeltaLink deltaLink);

}
