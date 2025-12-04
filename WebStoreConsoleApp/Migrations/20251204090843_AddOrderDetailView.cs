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
                    o.Id AS OrderId,
                    c.CustomerName,
                    o.OrderDate,
                    COUNT(od.OrderId) AS TotalRows,
                    SUM(od.Price * od.Quantity) AS TotalAmount
                FROM Orders AS o
                LEFT JOIN OrderDetail AS od ON od.OrderId = o.Id
                LEFT JOIN Customer AS c ON o.CustomerId = c.Id
                GROUP BY o.Id, c.CustomerName, o.OrderDate;
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
