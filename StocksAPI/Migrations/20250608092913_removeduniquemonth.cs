using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class removeduniquemonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MonthlyInvestments_MonthYear",
                table: "MonthlyInvestments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInvestments_MonthYear",
                table: "MonthlyInvestments",
                column: "MonthYear",
                unique: true);
        }
    }
}
