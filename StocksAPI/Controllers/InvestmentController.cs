using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StocksAPI.Models;
using StocksAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace StocksAPI.Controllers
{
    [Authorize]
    [Route("api/monthly-investments")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IDataRepository<MonthlyInvestment> _dataRepository;

        public InvestmentController(IDataRepository<MonthlyInvestment> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? throw new InvalidOperationException("User ID not found in token");
        }

        // GET: api/monthly-investments
        [HttpGet]
        public IActionResult Get()
        {
            var userId = GetCurrentUserId();
            IEnumerable<MonthlyInvestment> investments = _dataRepository.GetAll()
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.MonthYear);

            return Ok(investments);
        }

        // GET: api/monthly-investments/5
        [HttpGet("{id}", Name = "GetInvestment")]
        public IActionResult Get(long id)
        {
            var userId = GetCurrentUserId();
            MonthlyInvestment investment = _dataRepository.Get(id);

            if (investment == null)
            {
                return NotFound("Monthly investment record not found.");
            }

            if (investment.UserId != userId)
            {
                return Forbid();
            }

            return Ok(investment);
        }

        // POST: api/monthly-investments
        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var investments = new List<MonthlyInvestment>();
            var userId = GetCurrentUserId();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null) continue;

                    var dateCell = currentRow.GetCell(0);
                    var amountCell = currentRow.GetCell(1);

                    if (dateCell != null && amountCell != null)
                    {
                        var dateStr = Convert.ToString(dateCell);
                        var amountStr = Convert.ToString(amountCell);

                        if (string.IsNullOrEmpty(dateStr) || string.IsNullOrEmpty(amountStr))
                        {
                            continue;
                        }

                        if (!DateTime.TryParse(dateStr, out DateTime monthYear) || 
                            !decimal.TryParse(amountStr, out decimal amount))
                        {
                            return BadRequest($"Invalid data format in row {row + 1}");
                        }

                        var investment = new MonthlyInvestment
                        {
                            MonthYear = monthYear,
                            Amount = amount,
                            UserId = userId,
                            CreatedAt = DateTime.UtcNow
                        };

                        // Check for existing entry for this month and user
                        if (_dataRepository.GetAll().Any(m => 
                            m.MonthYear == investment.MonthYear && 
                            m.UserId == userId))
                        {
                            return BadRequest($"Entry for {investment.MonthYear:MMMM yyyy} already exists.");
                        }

                        investments.Add(investment);
                    }
                }
            }

            foreach (var investment in investments)
            {
                _dataRepository.Add(investment);
            }

            return Ok(new { Count = investments.Count, Message = "Investments added successfully." });
        }

        // POST: api/monthly-investments/single
        [HttpPost("single")]
        public IActionResult PostSingle([FromBody] MonthlyInvestment investment)
        {
            if (investment == null)
            {
                return BadRequest("Investment data is null.");
            }
            investment.UserId = GetCurrentUserId();
            // Additional validation for month/year uniqueness
            if (_dataRepository.GetAll().Any(m => m.MonthYear == investment.MonthYear && m.UserId == investment.UserId))
            {
                return BadRequest($"Entry for {investment.MonthYear} already exists.");
            }

            _dataRepository.Add(investment);
            return CreatedAtRoute("GetInvestment", new { id = investment.Id }, investment);
        }

        // PUT: api/monthly-investments/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] MonthlyInvestment investment)
        {
            if (investment == null)
            {
                return BadRequest("Investment data is null.");
            }

            MonthlyInvestment investmentToUpdate = _dataRepository.Get(id);
            if (investmentToUpdate == null)
            {
                return NotFound("Investment record not found.");
            }

            // Prevent duplicate month/year entries on update
            if (investment.MonthYear != investmentToUpdate.MonthYear &&
                _dataRepository.GetAll().Any(m => m.MonthYear == investment.MonthYear))
            {
                return BadRequest("Another entry already exists for this month/year.");
            }

            investmentToUpdate.MonthYear = investment.MonthYear;
            investmentToUpdate.Amount = investment.Amount;
            // investmentToUpdate.Addition = investment.Addition;
            // investmentToUpdate.PercentageChange = investment.PercentageChange;

            _dataRepository.Update(investmentToUpdate, investment);
            return NoContent();
        }

        // DELETE: api/monthly-investments/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            MonthlyInvestment investment = _dataRepository.Get(id);
            if (investment == null)
            {
                return NotFound("Investment record not found.");
            }

            _dataRepository.Delete(investment);
            return NoContent();
        }
    }
}
