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
            Console.WriteLine(
                $"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerAddress} | {customer.CustomerEmail}");
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

        Console.WriteLine("Please enter the address of the customer: ");
        var customerAddress = Console.ReadLine();

        if (string.IsNullOrEmpty(customerAddress) || customerAddress.Length > 50)
        {
            Console.WriteLine("Customer City is required, max 50.");
        }

        Console.WriteLine("Please enter the Email of the customer: ");
        var customerEmail = Console.ReadLine();

        if (string.IsNullOrEmpty(customerEmail) || customerEmail.Length > 50)
        {
            Console.WriteLine("Customer Email is required, max 50.");
        }

        using var db = new StoreContext();
        db.Customers.Add(new Customer
            { CustomerName = customerName, CustomerAddress = customerAddress, CustomerEmail = customerEmail });
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer added successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    ///  Edits an existing customer in the database.
    /// </summary>
    public static async Task CustomerEditAsync()
    {
        using var db = new StoreContext();

        await CustomerListAsync();
        Console.WriteLine(" ");
        Console.WriteLine("Enter Customer ID to update: ");


        if (!int.TryParse(Console.ReadLine(), out var customerId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.Write($"Name {customer.CustomerName}: ");
        var newName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(newName))
        {
            customer.CustomerName = newName;
        }

        Console.Write($"Adress {customer.CustomerAddress}: ");
        var newAddress = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(newAddress))
        {
            customer.CustomerAddress = newAddress;
        }

        Console.Write($"Email [{customer.CustomerEmail}]: ");
        var newEmail = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(newEmail))
        {
            customer.CustomerEmail = newEmail;
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

        await CustomerListAsync();
        Console.WriteLine(" ");

        Console.Write($"Enter Customer ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Customer not found.");
        }

        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
        }

        db.Customers.Remove(customer);

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer deleted successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }

    /// <summary>
    ///  List of total orders for customers
    /// </summary>
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

    public static async Task CustomerListandOrdersAsync()
    {
        using var db = new StoreContext();
        
        // Use this For-Loop to Loop customers and orders
        // This is commented-out to not loop new customrs and orders
        
        /*for (int i = 1; i <= 100; i++) // Create 5 orders
        {
            var customer = new Customer
            {
                CustomerName = $"Test Customer {i}",
                CustomerAddress = $"Street {i}",
               CustomerEmail = $"customer1{i}@mail.com",
                Orders = new List<Order>()
            };

            // Create 2 orders per customer
            for (int j = 1; j <= 2; j++) 
            {
                customer.Orders.Add(new Order
                {
                    OrderDate = DateTime.Now.AddDays(-j),
                    TotalAmount = 100 + j * 10,
                    OrderStatus = "Pending"
                });
            }

            db.Customers.Add(customer);
        }

        await db.SaveChangesAsync();*/
        
        var sw = System.Diagnostics.Stopwatch.StartNew();
        
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .Include(x => x.Orders)
            .ToListAsync();
        Console.WriteLine("Customers:");
        Console.WriteLine("ID | Name | City | Email | Number of Orders");

        sw.Stop();
        Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
        
        foreach (var customer in customers)
        {
            if (customer.Orders != null)
                Console.WriteLine(
                    $"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerAddress} | {customer.CustomerEmail} | {customer.Orders.Count}");
        }
    }
}