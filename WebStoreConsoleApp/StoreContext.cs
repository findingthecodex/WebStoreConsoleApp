using Microsoft.EntityFrameworkCore;
using WebStoreConsoleApp.Models;

namespace WebStoreConsoleApp;

public class StoreContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OrderSummary> OrderSummaries => Set<OrderSummary>();
    public DbSet<CustomerOrderCount> CustomerOrderCounts => Set<CustomerOrderCount>();
    public DbSet<ProductSalesView> ProductSalesViews => Set<ProductSalesView>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "WebStoreConsoleApp.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<OrderSummary>(o =>
            {
                o.HasNoKey(); // Saknar PK alltså har ingen primärnyckel
                o.ToView("OrderSummaryView"); // Koppla tabellen mot SQlite
            }
        );
        
        modelBuilder.Entity<CustomerOrderCount>(c=>
        {
            c.HasNoKey();
            c.ToView("CustomerOrderCountView");
        });

        modelBuilder.Entity<ProductSalesView>(p =>
        {
            p.HasNoKey();
            p.ToView("ProductSalesView");
        });
            
        modelBuilder.Entity<Customer>(c =>
        {
            c.HasKey(c => c.CustomerId);
            c.Property(x => x.CustomerName).IsRequired().HasMaxLength(50);
            c.Property(x => x.CustomerAddress).IsRequired().HasMaxLength(100);
            c.HasIndex(x => x.CustomerEmail).IsUnique();
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.HasKey(x => x.OrderId);
            o.Property(x => x.OrderDate).IsRequired();
            o.Property(x => x.OrderStatus).IsRequired();
            o.Property(x => x.TotalAmount);

            // Foreign Key Relation
            o.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            ;
        });

        modelBuilder.Entity<OrderRow>(o =>
        {
            o.HasKey(x => x.OrderRowId);
            o.Property(x => x.OrderRowQuantity).IsRequired();
            o.Property(x => x.OrderRowUnitPrice).IsRequired();

            // Foreign Key Relation
            o.HasOne(x => x.Order)
                .WithMany(x => x.OrderRows)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign Key Relation
            o.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(p =>
        {
            p.HasKey(x => x.ProductId);
            p.Property(x => x.ProductName).IsRequired().HasMaxLength(50);
            p.Property(x => x.ProductPrice).IsRequired();
            
            p.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(c =>
        {
            c.HasKey(x => x.CategoryId);
            c.Property(x => x.CategoryName).IsRequired().HasMaxLength(50);
            c.Property(x => x.CategoryDescription).IsRequired().HasMaxLength(200);
        });
    }
}