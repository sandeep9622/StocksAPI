using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StocksAPI.Models;
using StocksAPI.Models.Repository;

namespace StocksAPI.Controllers
{
    [Authorize]
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IDataRepository<Stock> _dataRepository;
        
        public StockController(IDataRepository<Stock> dataRepository)
        {
            _dataRepository = dataRepository;
        }



        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? throw new InvalidOperationException("User ID not found in token");
        }

        // GET: api/Stock
        [HttpGet]
        public IActionResult Get()
        {
            var userId = GetCurrentUserId();
            IEnumerable<Stock> stocks = _dataRepository.GetAll()
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.TotalInvestment);
            return Ok(stocks);
        }

        // GET: api/Stock/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(long id)
        {
            var userId = GetCurrentUserId();
            Stock stock = _dataRepository.Get(id);
            
            if (stock == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }

            if (stock.UserId != userId)
            {
                return Forbid();
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

            // Remove ModelState error for UserId if present
            if (ModelState.ContainsKey("UserId"))
                ModelState.Remove("UserId");

            // Remove UserId from ModelState validation errors if present in nested validation
            if (ModelState.TryGetValue("stock.UserId", out var entry))
                ModelState.Remove("stock.UserId");

            // Set UserId from token
            stock.UserId = GetCurrentUserId();
            stock.CreatedAt = DateTime.UtcNow;
            
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

            if (ModelState.ContainsKey("UserId"))
                ModelState.Remove("UserId");
            if (ModelState.TryGetValue("stock.UserId", out var entry))
                ModelState.Remove("stock.UserId");

            var userId = GetCurrentUserId();
            var stockToUpdate = _dataRepository.Get(id);
            
            if (stockToUpdate == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }

            if (stockToUpdate.UserId != userId)
            {
                return Forbid();
            }

            stock.UserId = userId;
            stock.UpdatedAt = DateTime.UtcNow;
            
            _dataRepository.Update(stockToUpdate, stock);
            return NoContent();
        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var userId = GetCurrentUserId();
            var stock = _dataRepository.Get(id);
            
            if (stock == null)
            {
                return NotFound("The Stock record couldn't be found.");
            }

            if (stock.UserId != userId)
            {
                return Forbid();
            }

            _dataRepository.Delete(stock);
            return NoContent();
        }
    }
}
