using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class CreateMonthlyInvestmentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyInvestments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Addition = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentageChange = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyInvestments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInvestments_MonthYear",
                table: "MonthlyInvestments",
                column: "MonthYear",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyInvestments");
        }
    }
}
