using Microsoft.EntityFrameworkCore;

namespace StocksAPI.Models
{
    public class StockAPIContext: DbContext
    {
        public StockAPIContext(DbContextOptions options) : base(options) { }
        public DbSet<Stock> Stocks { get; set; }
    }
}
