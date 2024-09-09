using Microsoft.AspNetCore.Mvc;
using StocksAPI.Models;
using StocksAPI.Models.Repository;

namespace StocksAPI.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IDataRepository<Stock> _dataRepository;
        public StockController(IDataRepository<Stock> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // GET: api/Stock
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Stock> Stocks = _dataRepository.GetAll().OrderByDescending(s => s.TotalInvestment);
            return Ok(Stocks);
        }
        // GET: api/Stock/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(long id)
        {
            Stock stock = _dataRepository.Get(id);
            if (stock == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }
            return Ok(stock);
        }
        // POST: api/Stock
        [HttpPost]
        public IActionResult Post([FromBody] Stock stock)
        {
            if (stock == null)
            {
                return BadRequest("Stock is null.");
            }
            _dataRepository.Add(stock);
            return CreatedAtRoute(
                  "Get",
                  new { Id = stock.Id },
                  stock);
        }
        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Stock stock)
        {
            if (stock == null)
            {
                return BadRequest("Stock is null.");
            }
            Stock StockToUpdate = _dataRepository.Get(id);
            if (StockToUpdate == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }
            _dataRepository.Update(StockToUpdate, stock);
            return NoContent();
        }
        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Stock stock = _dataRepository.Get(id);
            if (stock == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }
            _dataRepository.Delete(stock);
            return NoContent();
        }
    }
}
