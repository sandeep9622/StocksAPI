using StocksAPI.Models.Repository;

namespace StocksAPI.Models.DataManager
{
    public class MonthlyInvestmentManager : IDataRepository<MonthlyInvestment>
    {
        readonly StockAPIContext _context;

        public MonthlyInvestmentManager(StockAPIContext context)
        {
            _context = context;
        }

        public void Add(MonthlyInvestment entity)
        {
            _context.MonthlyInvestments.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(MonthlyInvestment entity)
        {
            _context.MonthlyInvestments.Remove(entity);
            _context.SaveChanges();
        }

        public MonthlyInvestment Get(long id)
        {
            return _context.MonthlyInvestments.First(e => e.Id == id);
        }

        public IEnumerable<MonthlyInvestment> GetAll()
        {
            return _context.MonthlyInvestments.ToList();
        }

        public void Update(MonthlyInvestment dbEntity, MonthlyInvestment entity)
        {
            dbEntity.MonthYear = entity.MonthYear;
            dbEntity.Amount = entity.Amount;
            // dbEntity.Addition = entity.Addition;
            // dbEntity.PercentageChange = entity.PercentageChange;
            _context.SaveChanges();
        }
    }
}
