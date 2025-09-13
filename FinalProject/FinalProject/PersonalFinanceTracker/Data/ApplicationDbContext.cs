using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionCategory> TransactionCategories => Set<TransactionCategory>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Account>()
                .HasIndex(a => new { a.Name, a.UserId }).IsUnique();

            b.Entity<Account>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<TransactionCategory>()
                .HasKey(tc => new { tc.TransactionId, tc.CategoryId });

            b.Entity<TransactionCategory>()
                .HasOne(tc => tc.Transaction)
                .WithMany(t => t.TransactionCategories)
                .HasForeignKey(tc => tc.TransactionId);

            b.Entity<TransactionCategory>()
                .HasOne(tc => tc.Category)
                .WithMany(c => c.TransactionCategories)
                .HasForeignKey(tc => tc.CategoryId);
        }
    }
}
