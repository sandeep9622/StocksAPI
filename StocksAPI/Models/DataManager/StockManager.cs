using StocksAPI.Models.Repository;

namespace StocksAPI.Models.DataManager
{
    public class StockManager : IDataRepository<Stock>
    {
        readonly StockAPIContext _context;
        public StockManager(StockAPIContext context)
        {
            _context = context;
        }
        public void Add(Stock entity)
        {
            _context.Stocks.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Stock entity)
        {
            _context.Stocks.Remove(entity);
            _context.SaveChanges();
        }

        public Stock Get(long id)
        {
            return _context.Stocks.First(e => e.Id == id);
        }

        public IEnumerable<Stock> GetAll()
        {
            return _context.Stocks.ToList();
        }

        public void Update(Stock dbEntity, Stock entity)
        {
            dbEntity.Id = entity.Id;
            dbEntity.Price = entity.Price;
            dbEntity.Quantity = entity.Quantity;
            dbEntity.CurrentPrice = entity.CurrentPrice;
            dbEntity.CurrentPrice = entity.CurrentPrice;
            dbEntity.MarketCap = entity.MarketCap;
            dbEntity.Sector = entity.Sector;
            dbEntity.StockName = entity.StockName;
            _context.SaveChanges();
        }
    }
}
