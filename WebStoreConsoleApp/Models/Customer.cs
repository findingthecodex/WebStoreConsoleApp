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

    private string? _customerEmail;

    [Required, MaxLength(50)]
    public string? CustomerEmail
    {
        get => _customerEmail == null ? null : EncryptionHelper.Decrypt(_customerEmail);
        set => _customerEmail = string.IsNullOrEmpty(value) ? null : EncryptionHelper.Encrypt(value);
    }
    
    // Navigation
    public List<Order>? Orders { get; set; } = new ();
}