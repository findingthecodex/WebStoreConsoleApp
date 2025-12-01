using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreConsoleApp.Models;

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
            .OrderBy(c => c.OrderId).Include(order => order.Customer)
            .ToListAsync();
        Console.WriteLine("Order-List:");
        Console.WriteLine("OrderID | Name | Product | OrderDate | TotalAmount | OrderStatus");

        foreach (var order in orders)
        {
            Console.WriteLine(
                $"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderDate} | {order.TotalAmount} | {order.OrderStatus}");
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
        Console.WriteLine("OrderID | ProductName | Quantity | Price per unit | Row Total | Order Total");
        foreach (var order in orderdetails)
        {
            if (order.OrderId == detailsId)
            {
                foreach (var orderRow in order.OrderRows!)
                {
                    var rowTotal = orderRow.OrderRowQuantity * orderRow.OrderRowUnitPrice;
                    var orderTotal = orderRow.OrderRowQuantity * rowTotal;
                    Console.WriteLine(
                        $"{order.OrderId} | {orderRow.Product?.ProductName} | {orderRow.OrderRowQuantity} | {orderRow.OrderRowUnitPrice} | {rowTotal}");
                    Console.WriteLine($"Total Amount: {orderTotal}");
                }
            }
        }
    }
    
    /// <summary>
    ///  Adds a new order to the database.
    /// </summary>
    public static async Task OrderAddAsync()
    {
        await CustomerService.CustomerListAsync();
        
        using var db = new StoreContext();
        
        Console.WriteLine(" ");
        Console.Write("Please enter the Customer ID for the new order: ");

        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Customer ID is required.");
            return;
        }

        var customer = await db.Customers.FindAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer ID is required.");
        }

        Console.WriteLine("Products available: ");
        var products = await db.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        foreach (var product in products)
        {
            Console.WriteLine(
                $"{product.ProductId} | {product.ProductName} | {product.ProductPrice} ");
        }

        var orderRows = new List<OrderRow>();

        while (true)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Add product to the order (ProductID): ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Product ID is required.");
                return;
            }

            var productToAdd = await db.Products.FindAsync(productId);
            if (productToAdd == null)
            {
                Console.WriteLine("Product ID is required.");
            }

            Console.WriteLine("Product: " + productToAdd?.ProductName + " | Price: " + productToAdd?.ProductPrice);

            Console.WriteLine(" ");
            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Quantity must be a positive integer.");
                continue;
            }

            var row = new OrderRow
            {
                ProductId = productToAdd.ProductId,
                OrderRowQuantity = quantity,
                OrderRowUnitPrice = productToAdd.ProductPrice
            };
            orderRows.Add(row);

            Console.WriteLine(" ");
            Console.WriteLine("Product(s) added to order: " + productToAdd?.ProductName + " | Quantity: " + quantity +
                              " | Unit Price: " + productToAdd?.ProductPrice);

            Console.WriteLine("Do you want to add more products? (y/n): ");
            var addMore = Console.ReadLine()?.Trim().ToLower();
            if (addMore == "y")
            {
                continue;
            }

            break;
        }

        if (!orderRows.Any())
        {
            Console.WriteLine("No products added to the order. Order creation cancelled.");
            return;
        }

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

        Console.WriteLine("Order Summary:");
        foreach (var x in orderRows)
        {
            Console.WriteLine("ProductID: " + x.ProductId + " | + Product(s): " + x.Product + " | Quantity: " +
                              x.OrderRowQuantity + " | Unit Price: " + x.OrderRowUnitPrice);
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

        var orders = await db.Orders
            .Include(o => o.Customer)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();

        Console.WriteLine("All orders: ");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");
        foreach (var order in orders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | Customer: {order.Customer?.CustomerName} | OrderDate: {order.OrderDate} | TotalAmount: {order.TotalAmount} | OrderStatus: {order.OrderStatus}");
        }

        Console.WriteLine();
        Console.WriteLine("Enter order status (Pending, Processing, Paid, Shipped, Delivered): ");
        var statusInput = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(statusInput))
        {
            Console.WriteLine("Order status is required.");
            return;
        }

        var filteredOrders = orders
            .Where(o => o.OrderStatus.Equals(statusInput, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (!filteredOrders.Any())
        {
            Console.WriteLine($"No orders found with status '{statusInput}'.");
            return;
        }

        Console.WriteLine($"\nOrders with status '{statusInput}': ");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");
        foreach (var order in filteredOrders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | Customer: {order.Customer?.CustomerName} | OrderDate: {order.OrderDate} | TotalAmount: {order.TotalAmount} | OrderStatus: {order.OrderStatus}");
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

        foreach (var summary in summaries)
        {
            Console.WriteLine($"{summary.OrderId} | {summary.OrderDate} | {summary.TotalAmount} | {summary.CustomerEmail}");
        }
    }
}