using System.ComponentModel.DataAnnotations;

namespace WebStoreConsoleApp.Models;

public class Category
{
    // Primary Key
    public int CategoryId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? CategoryName { get; set; }
    [MaxLength(200)]
    public string CategoryDescription { get; set; } = string.Empty;
    
    // Navigation
    public List<Product> Products { get; set; } = new();
}