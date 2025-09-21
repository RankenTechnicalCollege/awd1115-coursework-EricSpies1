using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ContactManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Family" },
                new Category { CategoryId = 2, Name = "Friends" },
                new Category { CategoryId = 3, Name = "Work" }
            );

            b.Entity<Contact>().HasData(
                new Contact
                {
                    ContactId = 1,
                    Firstname = "Ada",
                    Lastname = "Lovelace",
                    Phone = "555-0101",
                    Email = "ada@example.com",
                    CategoryId = 3,
                    DateAdded = DateTime.UtcNow
                }
            );
        }
    }
}
