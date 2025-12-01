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
            
            Category headphonesCategory;
            Category phonesCategory;
            Category tabletsCategory;
            Category laptopsCategory;
            Category accessoriesCategory;

            if (!await db.Categories.AnyAsync())
            {
                headphonesCategory = new Category { CategoryName = "Headphones" };
                phonesCategory = new Category { CategoryName = "Phones" };
                tabletsCategory = new Category { CategoryName = "Tablets" };
                laptopsCategory = new Category { CategoryName = "Laptops" };
                accessoriesCategory = new Category { CategoryName = "Accessories" };
                
                db.Categories.AddRange(
                    headphonesCategory,
                    phonesCategory,
                    tabletsCategory,
                    laptopsCategory,
                    accessoriesCategory
                );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded Categories");
            }
            else
            {
                headphonesCategory = await db.Categories.FirstAsync(c => c.CategoryName == "Headphones");
                phonesCategory = await db.Categories.FirstAsync(c => c.CategoryName == "Phones");
                tabletsCategory = await db.Categories.FirstAsync(c => c.CategoryName == "Tablets");
                laptopsCategory = await db.Categories.FirstAsync(c => c.CategoryName == "Laptops");
                accessoriesCategory = await db.Categories.FirstAsync(c => c.CategoryName == "Accessories");
            }

            if (!await db.Products.AnyAsync())
            {
                db.Products.AddRange(

                    // Headphones
                    new Product { ProductName = "AirPods 4", ProductPrice = 1695, Category = headphonesCategory },
                    new Product { ProductName = "AirPods Pro 3", ProductPrice = 2995, Category = headphonesCategory },
                    new Product { ProductName = "AirPods Max", ProductPrice = 6495, Category = headphonesCategory },

                    // Phones
                    new Product { ProductName = "iPhone 17", ProductPrice = 10995, Category = phonesCategory },
                    new Product { ProductName = "iPhone 17 Pro", ProductPrice = 13995, Category = phonesCategory },
                    new Product { ProductName = "iPhone 17 Pro Max", ProductPrice = 14995, Category = phonesCategory },

                    // Tablets
                    new Product { ProductName = "iPad", ProductPrice = 4995, Category = tabletsCategory },
                    new Product { ProductName = "iPad Air", ProductPrice = 7795, Category = tabletsCategory },
                    new Product { ProductName = "iPad Pro", ProductPrice = 11995, Category = tabletsCategory },

                    // Laptops
                    new Product { ProductName = "MacBook Air M4", ProductPrice = 13495, Category = laptopsCategory },
                    new Product { ProductName = "MacBook Pro M5", ProductPrice = 20995, Category = laptopsCategory },
                    new Product { ProductName = "MacBook Pro M5 Max", ProductPrice = 25995, Category = laptopsCategory },

                    // Accessories
                    new Product { ProductName = "MagSafe Charger", ProductPrice = 495, Category = accessoriesCategory },
                    new Product { ProductName = "Apple Pencil 3rd Gen", ProductPrice = 1395, Category = accessoriesCategory },
                    new Product { ProductName = "Magic Keyboard for iPad", ProductPrice = 2995, Category = accessoriesCategory }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded Products");
            }
        }
    }
}