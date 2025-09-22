using Microsoft.EntityFrameworkCore;
using SalesOrders.Models;

namespace SalesOrders.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Product>()
                .HasOne(p => p.Category)
                .WithOne(c => c.Product)
                .HasForeignKey<Product>(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Product>()
                .HasIndex(p => p.CategoryID)
                .IsUnique();

            b.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Accessories" },
                new Category { CategoryID = 2, CategoryName = "Bikes" },
                new Category { CategoryID = 3, CategoryName = "Clothing" },
                new Category { CategoryID = 4, CategoryName = "Components" },
                new Category { CategoryID = 5, CategoryName = "Car racks" },
                new Category { CategoryID = 6, CategoryName = "Wheels" }
            );

            b.Entity<Product>().HasData(
                // Category 1 – Accessories
                new Product
                {
                    ProductID = 1,
                    ProductName = "Viscount C-500 Wireless Bike Computer",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "34-Viscount-C-500-Wireless-Bike-Computer.jpg",
                    ProductPrice = 49m,
                    ProductQty = 30,
                    CategoryID = 1
                },
                // Category 2 – Bikes
                new Product
                {
                    ProductID = 2,
                    ProductName = "Viscount Mountain Bike",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "37-Viscount-Mountain-Bikes.jpg",
                    ProductPrice = 635m,
                    ProductQty = 5,
                    CategoryID = 2
                },
                // Category 3 – Clothing
                new Product
                {
                    ProductID = 3,
                    ProductName = "Ultra-Pro Rain Jacket",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "32-Ultra-Pro-Rain-Jacket.jpg",
                    ProductPrice = 85m,
                    ProductQty = 30,
                    CategoryID = 3
                },
                // Category 4 – Components
                new Product
                {
                    ProductID = 4,
                    ProductName = "AeroFlo ATB Wheels",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "01-AeroFlo-ATB-Wheels.jpg",
                    ProductPrice = 189m,
                    ProductQty = 40,
                    CategoryID = 4
                },
                // Category 5 – Car racks
                new Product
                {
                    ProductID = 5,
                    ProductName = "Road Warrior Hitch Pack",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "21-Road-Warrior-Hitch-Pack.jpg",
                    ProductPrice = 175m,
                    ProductQty = 6,
                    CategoryID = 5
                }
            );
        }
    }
}
