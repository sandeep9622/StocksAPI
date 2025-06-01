using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksAPI.Migrations
{
    public partial class AddUserIdToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, ensure we have an admin user
            migrationBuilder.Sql(@"
                DECLARE @AdminId nvarchar(450);
                SELECT TOP 1 @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com';

                IF @AdminId IS NULL
                BEGIN
                    SET @AdminId = 'admin-' + CAST(NEWID() AS nvarchar(36));
                    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
                        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, 
                        LockoutEnabled, AccessFailedCount, FirstName, LastName, CreatedAt, IsActive)
                    VALUES (@AdminId, 'admin@stocksapi.com', 'ADMIN@STOCKSAPI.COM', 'admin@stocksapi.com', 
                        'ADMIN@STOCKSAPI.COM', 1, 
                        'AQAAAAEAACcQAAAAEKWz6tE+M+m4SxvDjb0u6Qz6qAMqvBKX6K83tnvzWqOAgqN5DxxQ+VN5NFh6WihPUA==',
                        'SECURITYSTAMP', NEWID(), 0, 0, 1, 0, 'Admin', 'User', GETUTCDATE(), 1);

                    INSERT INTO AspNetUserRoles (UserId, RoleId)
                    SELECT @AdminId, Id FROM AspNetRoles WHERE Name = 'Admin';
                END
            ");

            // Add columns to Stocks table
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
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "(SELECT TOP 1 Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com')");

            // Add columns to MonthlyInvestments table
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
                type: "nvarchar(450)",
                nullable: false,
                defaultValueSql: "(SELECT TOP 1 Id FROM AspNetUsers WHERE Email = 'admin@stocksapi.com')");

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInvestments_UserId",
                table: "MonthlyInvestments",
                column: "UserId");

            // Add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInvestments_AspNetUsers_UserId",
                table: "MonthlyInvestments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInvestments_AspNetUsers_UserId",
                table: "MonthlyInvestments");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyInvestments_UserId",
                table: "MonthlyInvestments");

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
