using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDetailView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderDetailView AS
                SELECT
                    o.OrderId,
                    c.CustomerName,
                    o.OrderDate,
                    COUNT(orw.OrderId) AS TotalRows,
                    SUM(orw.OrderRowUnitPrice * orw.OrderRowQuantity) AS TotalAmount
                FROM Orders AS o
                LEFT JOIN OrderRows AS orw ON orw.OrderId = o.OrderId
                LEFT JOIN Customers AS c ON o.CustomerId = c.CustomerId
                GROUP BY o.OrderId, c.CustomerName, o.OrderDate;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderDetailView
            ");
        }
    }
}
