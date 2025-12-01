using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderTotalPrice",
                table: "Orders",
                newName: "TotalAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Orders",
                newName: "OrderTotalPrice");
        }
    }
}
