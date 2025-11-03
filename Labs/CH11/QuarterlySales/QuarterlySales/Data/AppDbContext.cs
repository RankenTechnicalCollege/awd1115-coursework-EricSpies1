using Microsoft.EntityFrameworkCore;
using QuarterlySales.Models;

namespace QuarterlySales.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<SalesEntry> Sales => Set<SalesEntry>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<SalesEntry>()
             .HasIndex(s => new { s.EmployeeId, s.Year, s.Quarter })
             .IsUnique();

            b.Entity<Employee>()
             .HasOne(e => e.Manager)
             .WithMany()
             .HasForeignKey(e => e.ManagerId)
             .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Joyce",
                    LastName = "Valdez",
                    DOB = new DateTime(1956, 12, 10),
                    DateOfHire = new DateTime(1995, 1, 1),
                    ManagerId = null
                }
            );
        }
    }
}
