using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksAPI.Models
{
    public class Stock : BaseEntity
    {
        [Required]
        public string StockName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Sector { get; set; }
        public decimal MarketCap { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal CurrentPrice { get; set; }

        [NotMapped]
        public decimal Returns
        {
            get
            {
                return Math.Round(CurrentPrice * Quantity - TotalInvestment, 2);
            }
        }

        public decimal? OneYearReturns { get; set; }

        public decimal? ThreeYearReturns { get; set; }

        public decimal? FiveYearReturns { get; set; }
    }
}
