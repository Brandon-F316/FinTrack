using FinTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Data;

public class FinTrackDbContext : DbContext
{
    public FinTrackDbContext(DbContextOptions<FinTrackDbContext> options)
    : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Holding> Holdings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Holding>()
            .HasOne(h => h.Portfolio)
            .WithMany()
            .HasForeignKey(h => h.PortfolioId);

        modelBuilder.Entity<Holding>()
            .HasOne(h => h.Stock)
            .WithMany()
            .HasForeignKey(h => h.StockId);
    }
    
}