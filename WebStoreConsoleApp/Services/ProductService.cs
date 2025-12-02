namespace WebStoreConsoleApp.Services;

public class ProductService
{
    /// <summary>
    ///  Lists all headphones in the database.
    /// </summary>
    public static async Task ListHeadPhonesAsync()
    {
        using var db = new StoreContext();
        var products = await db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == 1) // CategoryId 1 - Headphones
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("Headphones List:");
        Console.WriteLine("ProductID | ProductName | ProductPrice");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice}");
        }

        Console.WriteLine(" ");
    }

    /// <summary>
    ///  Lists all phones in the database.
    /// </summary>
    public static async Task ListPhonesAsync()
    {
        using var db = new StoreContext();
        var products = await db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == 2) // CategoryId 2 - Phones
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("Phones List:");
        Console.WriteLine("ProductID | ProductName | ProductPrice");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice}");
        }

        Console.WriteLine(" ");
    }
    
    /// <summary>
    ///  Lists all laptops in the database.
    /// </summary>
    public static async Task ListLaptopsAsync()
    {
        using var db = new StoreContext();
        var products = await db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == 3) // CategoryId 3 - Laptops
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("Laptops List:");
        Console.WriteLine("ProductID | ProductName | ProductPrice");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice}");
        }

        Console.WriteLine(" ");
    }
    
    /// <summary>
    ///  Lists all tablets in the database.
    /// </summary>
    public static async Task ListTabletsAsync()
    {
        using var db = new StoreContext();
        var products = await db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == 4) // CategoryId 4 - Tablets
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("Tablets List:");
        Console.WriteLine("ProductID | ProductName | ProductPrice");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice}");
        }

        Console.WriteLine(" ");
    }
    
    /// <summary>
    ///  Lists all accessories in the database.
    /// </summary>
    public static async Task ListAccessoriesAsync()
    {
        using var db = new StoreContext();
        var products = await db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == 5) // CategoryId 5 - Accessories
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("Accessories List:");
        Console.WriteLine("ProductID | ProductName | ProductPrice");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice}");
        }

        Console.WriteLine(" ");
    }
}