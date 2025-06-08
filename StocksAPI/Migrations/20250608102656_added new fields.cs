using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class addednewfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FiveYearReturns",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OneYearReturns",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThreeYearReturns",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FiveYearReturns",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "OneYearReturns",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ThreeYearReturns",
                table: "Stocks");
        }
    }
}
