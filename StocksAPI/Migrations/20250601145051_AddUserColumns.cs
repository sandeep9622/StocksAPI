using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class AddUserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stocks",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MonthlyInvestments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MonthlyInvestments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MonthlyInvestments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MonthlyInvestments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MonthlyInvestments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MonthlyInvestments");
        }
    }
}
