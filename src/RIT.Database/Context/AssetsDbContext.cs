using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RIT.Database.Entities;

namespace RIT.Database.Context;

public sealed class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
        : base(options) { }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<BankAsset> BankAssets { get; set; }
    public DbSet<CashAsset> CashAssets { get; set; }
    public DbSet<RealEstateAsset> RealEstateAssets { get; set; }
    public DbSet<InventoryItemAsset> InventoryItemAssets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("AssetsList");
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });
}
