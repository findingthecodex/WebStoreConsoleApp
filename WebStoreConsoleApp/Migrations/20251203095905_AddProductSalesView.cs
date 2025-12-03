using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSalesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS ProductSalesView AS
            SELECT
                p.ProductId,
                p.ProductName,
                IFNULL(SUM(orw.OrderRowQuantity), 0) AS TotalQuantitySold,
                IFNULL(SUM(orw.OrderRowQuantity * orw.OrderRowUnitPrice), 0) AS TotalSalesAmount
            FROM Products p
            LEFT JOIN OrderRows orw ON p.ProductId = orw.ProductId
            GROUP BY p.ProductId, p.ProductName;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS ProductSalesView;
            ");
        }
    }
}
