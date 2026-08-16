using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePortfolioStockIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Holdings_PortfolioId",
                table: "Holdings");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_PortfolioId_StockId",
                table: "Holdings",
                columns: new[] { "PortfolioId", "StockId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Holdings_PortfolioId_StockId",
                table: "Holdings");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_PortfolioId",
                table: "Holdings",
                column: "PortfolioId");
        }
    }
}
