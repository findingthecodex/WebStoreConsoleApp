using System.ComponentModel.DataAnnotations;

namespace WebStoreConsoleApp.Models;

public class Customer
{
    // Primary Key
    public int CustomerId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? CustomerName { get; set; }
    [Required, MaxLength(50)]
    public string? CustomerAddress { get; set; }
    [Required, MaxLength(50)]
    public string? CustomerEmail { get; set; }
    
    // Navigation
    public List<Order>? Orders { get; set; } = new ();
}