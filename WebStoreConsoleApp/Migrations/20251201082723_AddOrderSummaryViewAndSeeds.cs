using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
/*
 * En view är en sparad SELECT-fråga i db.
 * - Förenklar komplexa JOIN(s)
 * - Ger oss färdiga summeringar
 * - Slipper skriva samma SQL om och om igen
 * - Säker visning av information och prestandan blir bättre
 * 
 */
namespace WebStoreConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSummaryViewAndSeeds : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
       {
           migrationBuilder.Sql(@"
           CREATE VIEW IF NOT EXISTS OrderSummaryView AS
           SELECT
               o.OrderId,
               c.CustomerName,
               c.CustomerEmail,
               o.OrderDate,
               IFNULL(SUM(orw.OrderRowQuantity * orw.OrderRowUnitPrice), 0) AS TotalAmount
           FROM Orders o
           JOIN Customers c ON o.CustomerId = c.CustomerId
           LEFT JOIN OrderRows orw ON o.OrderId = orw.OrderId
           GROUP BY o.OrderId, c.CustomerName, c.CustomerEmail, o.OrderDate;
           ");
           
           // AFTER INSERT
           migrationBuilder.Sql(@"
           CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
           AFTER INSERT ON OrderRows
           BEGIN
               UPDATE Orders
               SET TotalAmount = (
                                       SELECT IFNULL(SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                                       FROM OrderRows
                                       WHERE OrderId = NEW.OrderId
                                    )
               WHERE OrderId = NEW.OrderId;
           END;
           ");
       
           // AFTER UPDATE
           migrationBuilder.Sql(@"
           CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Update
           AFTER UPDATE ON OrderRows
           BEGIN
               UPDATE Orders
               SET TotalAmount = (
                                       SELECT IFNULL(SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                                       FROM OrderRows WHERE OrderId = OLD.OrderId
                                       )
               WHERE OrderId = OLD.OrderId;
       
               UPDATE Orders
               SET TotalAmount = (
                                       SELECT IFNULL(SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                                       FROM OrderRows WHERE OrderId = NEW.OrderId
                                       )
               WHERE OrderId = NEW.OrderId;
           END;
           ");
       
           // AFTER DELETE
           migrationBuilder.Sql(@"
           CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Delete
           AFTER DELETE ON OrderRows
           BEGIN
               UPDATE Orders
               SET TotalAmount = (
                                       SELECT IFNULL(SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                                       FROM OrderRows
                                       WHERE OrderId = OLD.OrderId
                                       )
              WHERE OrderId = OLD.OrderId;
           END;
           ");
       }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderSummaryView
            ");

            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Insert
            ");
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Update
            ");
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Delete
            ");
        }
    }
}
