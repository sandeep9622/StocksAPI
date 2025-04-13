using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class CreateMonthlyInvestmentsTabledgf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Addition",
                table: "MonthlyInvestments");

            migrationBuilder.DropColumn(
                name: "PercentageChange",
                table: "MonthlyInvestments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Addition",
                table: "MonthlyInvestments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageChange",
                table: "MonthlyInvestments",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
