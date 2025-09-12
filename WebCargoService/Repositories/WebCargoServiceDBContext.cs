using Microsoft.EntityFrameworkCore;
using WebCargoService.Models.Entities;

namespace WebCargoService.Repositories;

public class WebCargoServiceDBContext(DbContextOptions<WebCargoServiceDBContext> options) : DbContext(options) {

    public DbSet<Rate> Rates { get; set; }

    public DbSet<RateBreakpoint> RateBreakpoints { get; set; }


    public DbSet<RateSurcharge> RateSurcharges { get; set; }

    public DbSet<RateCache> RateCaches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        ConfigureRateEntity(modelBuilder);
        ConfigureRateSurchargeEntity(modelBuilder);
        ConfigureRateCacheEntity(modelBuilder);
    }

    private static void ConfigureRateEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Rate>()
            .HasIndex(webCargoRate => webCargoRate.UniqueCode)
            .IsUnique();
    }

    private static void ConfigureRateSurchargeEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<RateSurcharge>()
            .Property(webCargoSurcharge => webCargoSurcharge.CostType)
            .HasConversion<string>()
            .HasMaxLength(50);
    }

    private static void ConfigureRateCacheEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<RateCache>()
            .HasKey(rateCache => new { rateCache.OriginAirportIATACode, rateCache.DestinationAirportIATACode });
    }

}
