using Microsoft.EntityFrameworkCore;
using WebStoreConsoleApp.Models;

namespace WebStoreConsoleApp.SeedData;

public class Seeds
{
    public static async Task MigrateDatabaseAsync()
    {
        Console.WriteLine("Db: " + Path.Combine(AppContext.BaseDirectory, "WebStoreConsoleApp.db"));
        using (var db = new StoreContext())

        {
            await db.Database.MigrateAsync();

            if (!await db.Customers.AnyAsync())
            {
                db.Customers.AddRange(
                    new Customer { CustomerName = "Amanda Ed", CustomerAddress = "Fjällgatan 1", CustomerEmail = "amanda.ed@gmail.com"},
                    new Customer { CustomerName = "James Gregory", CustomerAddress = "Kalkstigen 12", CustomerEmail = "james.gregory@gmail.com"},
                    new Customer { CustomerName = "Anna Ditcheva", CustomerAddress = "Skogstigen 7", CustomerEmail = "anna.ditcheva@gmail.com"},
                    new Customer { CustomerName = "Björn Borg", CustomerAddress = "Tennisvägen 2", CustomerEmail = "bjorn.borg@gmail.com"}
                ); 
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded Customers");
            }
            
            if (!await db.Products.AnyAsync())
            {
                db.Products.AddRange(
                    
                    // Headphones
                    new Product { ProductName = "AirPods 4", ProductPrice = 1695, Category = headphonesCategory },
                    new Product { ProductName = "AirPods Pro 3", ProductPrice = 2995 },
                    new Product { ProductName = "AirPods Max", ProductPrice = 6495 },
                    
                    // Phones
                    new Product { ProductName = "iPhone 17", ProductPrice = 10995 },
                    new Product { ProductName = "iPhone 17 Pro", ProductPrice = 13995 },
                    new Product { ProductName = "iPhone 17 Pro Max", ProductPrice = 14995 },
                    
                    // Tablets
                    new Product { ProductName = "iPad", ProductPrice = 4995 },
                    new Product { ProductName = "iPad Air", ProductPrice = 7795 },
                    new Product { ProductName = "iPad Pro", ProductPrice = 11995 },
                    
                    // Laptops
                    new Product { ProductName = "MacBook Air M4", ProductPrice = 13495 },
                    new Product { ProductName = "MacBook Pro M5", ProductPrice = 20995 },
                    new Product { ProductName = "MacBook Pro M5 Max", ProductPrice = 25995 },
                    
                    // Accessories
                    new Product { ProductName = "MagSafe Charger", ProductPrice = 495 },
                    new Product { ProductName = "Apple Pencil 3rd Gen", ProductPrice = 1395 },
                    new Product { ProductName = "Magic Keyboard for iPad", ProductPrice = 2995 }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded Products");
            }

            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new Category { CategoryName = "Headphones" },
                    new Category { CategoryName = "Phones" },
                    new Category { CategoryName = "Tablets" },
                    new Category { CategoryName = "Laptops" },
                    new Category { CategoryName = "Accessories" }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded Categories");
            }
        }
    }
}