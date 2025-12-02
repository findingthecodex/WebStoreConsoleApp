

namespace WebStoreConsoleApp.Services;

public class OrderService
{
    /// <summary>
    ///  Lists all orders in the database.
    /// </summary>
    public static async Task OrderListAsync()
    {
        using var db = new StoreContext();
        var orders = await db.Orders
            .AsNoTracking()
            .OrderBy(c => c.OrderId)
            .Include(order => order.Customer)
            .ToListAsync();
        Console.WriteLine("Order-List:");
        Console.WriteLine("OrderID | Name | Product | OrderDate | TotalAmount | OrderStatus");

        var culture = new CultureInfo("sv-SE");
        foreach (var order in orders)
        {
            Console.WriteLine(
                $"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderDate} | {order.TotalAmount.ToString("C", culture)} | {order.OrderStatus}");
        }
    }

    /// <summary>
    ///  Lists order details for a specific order ID.
    /// </summary>
    /// <param name="detailsId"></param>
    public static async Task OrderDetailsAsync(int detailsId)
    {
        using var db = new StoreContext();

        var orderdetails = await db.Orders
            .AsNoTracking()
            .OrderBy(x => x.OrderId)
            .Include(o => o.OrderRows)!
            .ThenInclude(x => x.Product)
            .ToListAsync();
        Console.WriteLine("Order Details:");
        Console.WriteLine("OrderID | ProductName | Quantity | Price");
        foreach (var order in orderdetails)
        {
            if (order.OrderId == detailsId)
            {
                var culture = new CultureInfo("sv-SE");
                foreach (var orderRow in order.OrderRows!)
                {
                    var rowTotal = orderRow.OrderRowQuantity * orderRow.OrderRowUnitPrice;
                    var orderTotal = orderRow.OrderRowQuantity * rowTotal;
                    Console.WriteLine(
                        $"{order.OrderId} | {orderRow.Product?.ProductName} | {orderRow.OrderRowQuantity} | {orderRow.OrderRowUnitPrice.ToString("C", culture)}");
                }
                Console.WriteLine(" ");
                Console.WriteLine($"Total Amount: {order.TotalAmount.ToString("C", culture)}");
            }
        }
    }

    /// <summary>
    ///  Adds a new order to the database.
    /// </summary>
    /// <summary>
    ///  Adds a new order to the database.
    /// </summary>
    public static async Task OrderAddAsync()
    {
        using var db = new StoreContext();
        var culture = new CultureInfo("sv-SE");
         
        await CustomerService.CustomerListAsync();

        // Choose customer
        int customerId;
        Customer? customer;
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Enter Customer ID for the new order (or type EXIT to cancel): ");
            var input = Console.ReadLine()?.Trim();

            if (input?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
            {
                Console.WriteLine("Order cancelled.");
                return;
            }

            if (!int.TryParse(input, out customerId))
            {
                Console.WriteLine("Invalid Customer ID. Try again.");
                continue;
            }

            customer = await db.Customers.FindAsync(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found. Try again.");
                continue;
            }

            break;
        }

        var orderRows = new List<OrderRow>();
    
        while (true) // Loop to add multiple products
        {
            // --- Step 1 - Chose category ---
            var categories = await db.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryId)
                .ToListAsync();

            Console.WriteLine("\nAvailable Categories:");
            foreach (var c in categories)
            {
                Console.WriteLine($"{c.CategoryId} | {c.CategoryName}");
            }
            Console.WriteLine(" ");

            int categoryId;
            while (true)
            {
                Console.WriteLine("Select a category (or type EXIT to cancel): ");
                var catInput = Console.ReadLine()?.Trim();

                if (catInput?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Order cancelled.");
                    return;
                }

                if (!int.TryParse(catInput, out categoryId) || !categories.Any(c => c.CategoryId == categoryId))
                {
                    Console.WriteLine("Invalid category. Try again.");
                    continue;
                }
                break;
            }

            // --- Step 2: Select products ---
            var products = await db.Products
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.ProductName)
                .ToListAsync();

            Console.WriteLine("\nProducts in selected category:");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.ProductId} | {p.ProductName} | {p.ProductPrice.ToString("C", culture)}");
            }
            Console.WriteLine(" ");
        
            Product? productToAdd = null;
            while (true)
            {
                Console.WriteLine("Select a product (type BACK for categories or EXIT to cancel): ");
                var prodInput = Console.ReadLine()?.Trim();

                if (prodInput?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Order cancelled.");
                    return;
                }
                

                if (prodInput?.Equals("back", StringComparison.OrdinalIgnoreCase) == true)
                {
                    productToAdd = null;
                    break;
                }
                Console.WriteLine(" ");

                if (!int.TryParse(prodInput, out int productId) || !products.Any(p => p.ProductId == productId))
                {
                    Console.WriteLine("Invalid product. Try again.");
                    continue;
                }

                productToAdd = products.First(p => p.ProductId == productId);
                break;
            }

            if (productToAdd == null)
            {
                continue;
            }
        
            // --- Step 3: Select quantity ---
            int quantity;
            while (true)
            {
                Console.WriteLine($"Enter quantity for {productToAdd.ProductName} (Type EXIT to cancel): ");
                var qtyInput = Console.ReadLine()?.Trim();

                if (qtyInput?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Order cancelled.");
                    return;
                }

                if (!int.TryParse(qtyInput, out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Quantity must be a positive number. Try again.");
                    continue;
                }
                break;
            }

            orderRows.Add(new OrderRow
            {
                ProductId = productToAdd.ProductId,
                OrderRowQuantity = quantity,
                OrderRowUnitPrice = productToAdd.ProductPrice
            });

            Console.WriteLine($"Added: {productToAdd.ProductName} | Quantity: {quantity} | Unit Price: {productToAdd.ProductPrice.ToString("C", culture)}");

            // Ask to add more products
            Console.Write("Do you want to add more products? (y/n): ");
            var addMore = Console.ReadLine()?.Trim().ToLower();
            if (addMore != "y") break;
        }

        if (!orderRows.Any())
        {
            Console.WriteLine("No products added. Order cancelled.");
            return;
        }

        // Save order
        decimal total = orderRows.Sum(x => x.OrderRowUnitPrice * x.OrderRowQuantity);

        var newOrder = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            OrderStatus = "Pending",
            TotalAmount = total,
            OrderRows = orderRows
        };

        db.Orders.Add(newOrder);
        await db.SaveChangesAsync();

        // Show order summary
        Console.WriteLine("\nOrder Summary:");
        foreach (var x in orderRows)
        {
            var prod = await db.Products.FindAsync(x.ProductId);
            Console.WriteLine($"Product: {prod?.ProductName} | Quantity: {x.OrderRowQuantity} | Unit Price: {x.OrderRowUnitPrice.ToString("C", culture)}");
        }

        Console.WriteLine($"\nTOTAL ORDER SUM: {total}");
        Console.WriteLine($"Order saved with OrderId: {newOrder.OrderId}");
    }
    
    /// <summary>
    ///  Lists orders filtered by their status.
    /// </summary>
    public static async Task OrderByStatusAsync()
{
    using var db = new StoreContext();

    while (true)
    {
        var orders = await db.Orders
            .Include(o => o.Customer)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();

        Console.WriteLine("All orders:");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");

        foreach (var order in orders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | " +
                $"Customer: {order.Customer?.CustomerName} | " +
                $"OrderDate: {order.OrderDate} | " +
                $"TotalAmount: {order.TotalAmount} | " +
                $"OrderStatus: {order.OrderStatus}");
        }

        Console.WriteLine();
        Console.WriteLine("Select an order status:");
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. Processing");
        Console.WriteLine("3. Paid");
        Console.WriteLine("4. Shipped");
        Console.WriteLine("5. Delivered");
        Console.WriteLine("0. Exit");

        var input = Console.ReadLine()?.Trim();

        if (input == "0")
        {
            Console.WriteLine("Exiting..");
            return;
        }

        string? statusInput = input switch
        {
            "1" => "Pending",
            "2" => "Processing",
            "3" => "Paid",
            "4" => "Shipped",
            "5" => "Delivered",
            _ => null
        };

        if (statusInput == null)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            continue;
        }

        var filteredOrders = orders
            .Where(o => o.OrderStatus != null && o.OrderStatus.Equals(statusInput, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!filteredOrders.Any())
        {
            Console.WriteLine($"No orders found with status '{statusInput}'.");
            continue;
        }

        Console.WriteLine($"\nOrders with status '{statusInput}':");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");

        foreach (var order in filteredOrders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | " +
                $"Customer: {order.Customer?.CustomerName} | " +
                $"OrderDate: {order.OrderDate} | " +
                $"TotalAmount: {order.TotalAmount} | " +
                $"OrderStatus: {order.OrderStatus}");
        }

        Console.WriteLine();
    }
}

    /// <summary>
    ///  Lists order summaries including customer email.
    /// </summary>
    public static async Task ListOrdersSummary()
    {
        using var db = new StoreContext();

        var summaries = await db.OrderSummaries.OrderBy(o => o.OrderId).ToListAsync();
        Console.WriteLine("Order-Summary:");
        Console.WriteLine("Order ID | OrderDate | TotalAmount SEK | Customer Email:");

        var culture = new CultureInfo("sv-SE");
        foreach (var summary in summaries)
        {
            Console.WriteLine($"{summary.OrderId} | {summary.OrderDate} | {summary.TotalAmount.ToString("C", culture)} | {summary.CustomerEmail}");
        }
    }

    public static async Task OrderDeleteAsync()
    {
        using var db = new StoreContext();

        var orders = await db.Orders
            .AsNoTracking()
            .OrderBy(o => o.OrderId)
            .Include(order => order.Customer)
            .ToListAsync();
        Console.WriteLine("Orders: ");
        Console.WriteLine("OrderID | OrderDate | TotalAmount | OrderStatus");
        var culture = new CultureInfo("sv-SE");
        foreach (var order in orders)
        {
            Console.WriteLine(
                $"{order.OrderId} | {order.OrderDate} | {order.TotalAmount.ToString("C", culture)} | {order.OrderStatus}");
        }

        Console.Write("Please enter the Order ID to delete: ");
        Console.WriteLine("(Type EXIT to cancel)");

        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }
        var orderToDelete = await db.Orders.FindAsync(orderId);
        if (orderToDelete == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        Console.WriteLine("Are you sure you want to delete the order with ID " + orderId + "? (y/n): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();
        if (confirmation != "y")
        {
            Console.WriteLine("Order deletion cancelled.");
            return;
        }
        db.Orders.Remove(orderToDelete);
        await db.SaveChangesAsync();
        Console.WriteLine($"Order with ID {orderId} has been deleted.");
    }
}