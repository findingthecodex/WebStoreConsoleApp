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
                o.OrderDate,
                c.CustomerName AS CustomerName,
                c.CustomerEmail AS CustomerEmail,
                IFNULL(SUM(orw.OrderRowQuantity * orw.OrderRowUnitPrice), 0) AS TotalPrice
            FROM Orders o
            JOIN Customers c ON c.CustomerId = o.CustomerId
            LEFT JOIN OrderRows orw ON orw.OrderId = o.OrderId
            GROUP BY o.OrderId, o.OrderDate, c.CustomerName, c.CustomerEmail;
            ");
            
            //Trigger
            
            // AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
            AFTER INSERT ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL(SUM(OrderRowQuantity * OrderRowUnitPrice), 0) 
                                    FROM OrderRows
                                    WHERE OrderId = NEW.OrderId;
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
                                    SELECT IFNULL(SUM(Quantity * UnitPrice), 0)
                                    FROM OrderRows
                                    WHERE OrderId = NEW.OrderId
                                    )
               WHERE OrderId = NEW.OrderId;
               END;        
            ");
            
            //CustomerOrderCountView
            //CREATE VIEW IF NOT EXISTS OrderSummaryView AS

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
