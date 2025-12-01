using System.ComponentModel.DataAnnotations;

namespace WebStoreConsoleApp.Models;

public class Order
{
    // Primary Key
    public int OrderId { get; set; }
    
    // Properties
    [Required]
    public DateTime OrderDate { get; set; }
    
    [Required]
    public string? OrderStatus { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    // Foreign Key
    public int CustomerId { get; set; }
    
    // Navigation
    public Customer? Customer { get; set; }

    public List<OrderRow>? OrderRows { get; set; } = new();
}