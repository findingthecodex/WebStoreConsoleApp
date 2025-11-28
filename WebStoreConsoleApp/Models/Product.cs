using System.ComponentModel.DataAnnotations;

namespace WebStoreConsoleApp.Models;

public class Product
{
    // Primary Key
    public int ProductId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? ProductName { get; set; }
    [Required]
    public int ProductPrice { get; set; }
    
    // Foreign key
    public int CategoryId { get; set; }
    
    // Navigation
    public Category? Category { get; set; }
}