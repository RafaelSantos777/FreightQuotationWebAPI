using Microsoft.EntityFrameworkCore;
using UserService.Models.Entities;

namespace UserService.Repositories;

public class UserServiceDBContext(DbContextOptions<UserServiceDBContext> options) : DbContext(options) {

    public DbSet<DeltaLink> DeltaLinks { get; set; }

}
