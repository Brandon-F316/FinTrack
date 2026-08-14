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

    
}