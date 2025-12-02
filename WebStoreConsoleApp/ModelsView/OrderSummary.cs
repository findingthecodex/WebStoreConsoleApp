namespace WebStoreConsoleApp.Models;

// Detta är en keyless entitet (INGEN PK)
// Den presenterar en SQL View, en spara SELECT-query
// Vi använder dessa Views i EF Core som gör att den kan läsa precis som en vanlig tabell

[Keyless] // Frivillig
public class OrderSummary
{
    public int OrderId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public string CustomerEmail { get; set; } = string.Empty;
    
    public decimal TotalAmount { get; set; }
}