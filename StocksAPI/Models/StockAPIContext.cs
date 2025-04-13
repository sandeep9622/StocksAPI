using Microsoft.EntityFrameworkCore;

namespace StocksAPI.Models
{
    public class StockAPIContext: DbContext
    {
        public StockAPIContext(DbContextOptions options) : base(options) { }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MonthlyInvestment> MonthlyInvestments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthlyInvestment>(entity =>
            {
                entity.HasIndex(e => e.MonthYear).IsUnique();
                entity.Property(e => e.MonthYear).HasColumnType("datetime2");
            });
        }
    }
}
