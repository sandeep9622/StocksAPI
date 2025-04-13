using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StocksAPI.Models
{
    public class MonthlyInvestment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime MonthYear { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        // [Required]
        // [Column(TypeName = "decimal(18,2)")]
        // public decimal Addition { get; set; }
        // [Required]
        // [Column(TypeName = "decimal(5,2)")]
        // public decimal PercentageChange { get; set; }

        [NotMapped]
        public string Month => MonthYear.ToString("MMMM");

        [NotMapped]
        public int Year => MonthYear.Year;
    }
}
