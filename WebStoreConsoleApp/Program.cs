
public class Program
{
    public static async Task Main(string[] args)
    {
        await Seeds.MigrateDatabaseAsync();
        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Orders");
            Console.WriteLine("3. Products");
            Console.WriteLine("Exit- Shutdown");
            Console.WriteLine(" ");

            var choice = Console.ReadLine();
            if (choice == "1")
                await CustomerMenu();
            else if (choice == "2")
                await OrderMenu();
            else if (choice == "3")
                await ProductMenu();
            else if (choice != null && (choice.Equals("Exit", StringComparison.OrdinalIgnoreCase) || choice.Equals("Shutdown", StringComparison.OrdinalIgnoreCase)))
                break;
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        static async Task CustomerMenu()
        {
            while (true)
            {
                Console.WriteLine("\nCustomers: 1. List | 2. Add | 3. Edit (3 <id>) | 4. Delete | 5. Customer-Orders-Count | 0. Exit");
                Console.WriteLine(" ");
                var line = Console.ReadLine()?.Trim() ?? string.Empty;

                if (line.Equals("..", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                switch (cmd)
                {
                    case "1":
                        await CustomerService.CustomerListAsync();
                        break;
                    case "2":
                        await CustomerService.CustomerAddAsync();
                        break;
                    case "3":
                        await CustomerService.CustomerListAsync();

                        if (parts.Length < 2 || !int.TryParse(parts[1], out int editId))
                        {
                            Console.WriteLine("Please provide a valid Customer ID to view details.");
                            break;
                        }

                        await CustomerService.CustomerEditAsync(editId);
                        break;
                    case "4":
                        await CustomerService.CustomerDeleteAsync();
                        break;
                    case "5":
                        await CustomerService.CustomerOrderCountAsync();
                        return;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        static async Task OrderMenu()
        {
            while (true)
            {
                Console.WriteLine(
                    "\nOrders: 1. Order-List | 2. Order-Details | 3. New-Order | 4. Status | 5. Order-Summary | 6. Delete-Order | 0. Exit");
                Console.WriteLine(" ");
                var line = Console.ReadLine()?.Trim() ?? string.Empty;

                if (line.Equals("..", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                switch (cmd)
                {
                    case "1":
                        await OrderService.OrderListAsync();
                        break;
                    case "2":

                        if (parts.Length < 2 || !int.TryParse(parts[1], out int detailsId))
                        {
                            Console.WriteLine("Please provide a valid Customer ID to view details.");
                            break;
                        }
                        await OrderService.OrderDetailsAsync(detailsId);
                        break;
                    case "3":
                        await OrderService.OrderAddAsync();
                        break;
                    case "4":
                        await OrderService.OrderByStatusAsync();
                        break;
                    case "5":
                        await OrderService.ListOrdersSummary();
                        break;
                    case"6":
                        await OrderService.OrderDeleteAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        static async Task ProductMenu()
        {
            while (true)
            {
                Console.WriteLine(
                    "\nCategories : 1. Headphones | 2. Phones | 3. Laptops | 4. Tablets | 5. Accessories | 6. New-Order | 7. Product-Sales-View | 0. Exit");
                Console.WriteLine(" ");
                var line = Console.ReadLine()?.Trim() ?? string.Empty;
                var parts = line.Split(' ');
                var cmd = parts[0].ToLowerInvariant();
                
                if (cmd == "0")
                {
                    Console.WriteLine("Exiting..");
                    return;
                }

                switch (cmd)
                {
                    case "1":
                        await ProductService.ListHeadPhonesAsync();
                        break;
                    case "2":
                        await ProductService.ListPhonesAsync();
                        break;
                    case "3":
                        await ProductService.ListTabletsAsync();
                        break;
                    case "4":
                        await ProductService.ListLaptopsAsync();
                        break;
                    case "5":
                        await ProductService.ListAccessoriesAsync();
                        break;
                    case "6":
                        await OrderService.OrderAddAsync();
                        break;
                    case "7":
                        await ProductService.ProductSalesViewAsync();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
        }
    }
}

