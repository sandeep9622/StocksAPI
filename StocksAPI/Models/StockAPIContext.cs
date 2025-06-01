using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StocksAPI.Models
{
    public class StockAPIContext : IdentityDbContext<ApplicationUser>
    {
        public StockAPIContext(DbContextOptions options) : base(options) { }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MonthlyInvestment> MonthlyInvestments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<MonthlyInvestment>(entity =>
            {
                entity.HasIndex(e => e.MonthYear).IsUnique();
                entity.Property(e => e.MonthYear).HasColumnType("datetime2");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MarketCap).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalInvestment).HasColumnType("decimal(18,2)");
            });
        }
    }
}
