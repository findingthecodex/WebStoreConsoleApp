namespace WebStoreConsoleApp.Models;

[Keyless]
public class ProductSales
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalSalesAmount { get; set; }
}