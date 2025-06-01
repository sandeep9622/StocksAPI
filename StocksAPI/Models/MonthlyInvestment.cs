using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StocksAPI.Models
{
    public class MonthlyInvestment : BaseEntity
    {
        [Required]
        public DateTime MonthYear { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [NotMapped]
        public string Month => MonthYear.ToString("MMMM");

        [NotMapped]
        public int Year => MonthYear.Year;
    }
}
