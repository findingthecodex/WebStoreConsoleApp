namespace WebStoreConsoleApp.Services;

public class CustomerService
{   
    /// <summary>
    ///  Lists all customers in the database.
    /// </summary>
    public static async Task CustomerListAsync()
    {
        using var db = new StoreContext();
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .ToListAsync();
        Console.WriteLine("Customers:");
        Console.WriteLine("ID | Name | City | Email");
        
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerAddress} | {customer.CustomerEmail}");
        }
    }
    
    /// <summary>
    ///  Adds a new customer to the database.
    /// </summary>
    public static async Task CustomerAddAsync()
    {
        
        Console.Write("Please enter the name of the customer: ");
        Console.WriteLine("(Type EXIT to cancel)");
        var customerName = Console.ReadLine()?.Trim() ?? string.Empty.ToLowerInvariant();
        
        if (customerName.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Customer addition cancelled.");
            return;
        }

        if (string.IsNullOrEmpty(customerName) || customerName.Length > 50)
        {
            Console.WriteLine("Customer Name is required, max 50.");
        }
        
        Console.Write("Please enter the address of the customer: ");
        var customerAddress = Console.ReadLine();
        
        if (string.IsNullOrEmpty(customerAddress) || customerAddress.Length > 50)
        {
            Console.WriteLine("Customer City is required, max 50.");
        }
        
        Console.Write("Please enter the Email of the customer: ");
        var customerEmail = Console.ReadLine();
        
        if (string.IsNullOrEmpty(customerEmail) || customerEmail.Length > 50)
        {
            Console.WriteLine("Customer Email is required, max 50.");
        }
        
        using var db = new StoreContext();
        db.Customers.Add(new Customer {CustomerName = customerName, CustomerAddress = customerAddress, CustomerEmail = customerEmail});
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer added successfully.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding customer: {ex.InnerException?.Message ?? ex.Message}");
        }
    }
    
    /// <summary>
    ///  Edits an existing customer in the database.
    /// </summary>
    /// <param name="customerId"></param>
    public static async Task CustomerEditAsync(int customerId)
    {
        using var db = new StoreContext();
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == 1);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
         
        Console.Write($"{customer.CustomerName}");
        var customername = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customername))
        {
            customername = customer.CustomerName;
        }
        
        Console.Write($"{customer.CustomerEmail}");
        var customercity = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customercity))
        {
            customercity = customer.CustomerEmail;
        }
        
        Console.Write($"{customer.CustomerAddress}");
        var customeremail = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customeremail))
        {
            customeremail = customer.CustomerEmail;
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer updated successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }

    /// <summary>
    ///  Deletes a customer from the database.
    /// </summary>
    public static async Task CustomerDeleteAsync()
    {
        using var db = new StoreContext();
        
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == 1);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
        db.Customers.Remove(customer);
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer deleted successfully.");
        }
        catch (DbUpdateException exeption)
        {
            Console.WriteLine(exeption.Message);
        }
    }

    public static async Task CustomerOrderCountAsync()
    {
        using var db = new StoreContext();
        
        var customerOrderCounts = await db.CustomerOrderCounts
            .AsNoTracking()
            .OrderByDescending(c => c.CustomerId)
            .ToListAsync();
        Console.WriteLine("Customer Order Counts:");
        Console.WriteLine("ID | Name | Email | Number of Orders");
        foreach (var coc in customerOrderCounts)
        {
            Console.WriteLine($"{coc.CustomerId} | {coc.CustomerName} | {coc.CustomerEmail} | {coc.NumberOfOrders}");
        }
    }
}