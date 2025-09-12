using Microsoft.EntityFrameworkCore;
using QuotationService.Models.Entities;

namespace QuotationService.Repositories;

public class QuotationServiceDBContext(DbContextOptions<QuotationServiceDBContext> options) : DbContext(options) {

    public DbSet<AirQuote> AirQuotes { get; set; }

    public DbSet<AirQuoteRequest> AirQuoteRequests { get; set; }

    public DbSet<AirQuoteResponse> AirQuoteResponses { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<SpecialHandlingCode> SpecialHandlingCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        ConfigureAirQuoteRequestEntity(modelBuilder);
        ConfigureLocationEntity(modelBuilder);
        ConfigureSpecialHandlingCodeEntity(modelBuilder);
    }

    private static void ConfigureAirQuoteRequestEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AirQuoteRequest>()
            .HasOne(airQuoteRequest => airQuoteRequest.OriginAirport)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AirQuoteRequest>()
            .HasOne(airQuoteRequest => airQuoteRequest.DestinationAirport)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureLocationEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Location>()
            .HasDiscriminator<string>("Type")
            .HasValue<Seaport>(nameof(Seaport))
            .HasValue<Airport>(nameof(Airport));
        modelBuilder.Entity<Airport>()
            .HasIndex(airport => airport.IATACode)
            .IsUnique();
        modelBuilder.Entity<Seaport>()
            .HasIndex(seaport => seaport.CargofiveLocationId)
            .IsUnique();
    }

    private static void ConfigureSpecialHandlingCodeEntity(ModelBuilder modelBuilder) {
        modelBuilder.Entity<SpecialHandlingCode>()
            .HasIndex(iataSpecialHandlingCode => iataSpecialHandlingCode.Code)
            .IsUnique();
    }

}
