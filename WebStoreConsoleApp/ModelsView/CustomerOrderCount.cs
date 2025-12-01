using Microsoft.EntityFrameworkCore;

namespace WebStoreConsoleApp.Models;

[Keyless]
public class CustomerOrderCount
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public int NumberOfOrders { get; set; }
}