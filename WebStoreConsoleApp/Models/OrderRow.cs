using System.ComponentModel.DataAnnotations;

namespace WebStoreConsoleApp.Models;

public class OrderRow
{
    // Primary Key
    public int OrderRowId { get; set; }
    
    // Properties
    [Required]
    public int OrderRowQuantity { get; set; }
    [Required]
    public decimal OrderRowUnitPrice { get; set; }
    
    // Foreign Key
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    
    // Navigation
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}