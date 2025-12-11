using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerName",
                table: "Customers",
                column: "CustomerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerName",
                table: "Customers");
        }
    }
}
