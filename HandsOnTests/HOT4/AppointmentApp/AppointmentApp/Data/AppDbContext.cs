using AppointmentApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Appointment>()
                .HasIndex(a => a.Start)
                .IsUnique();

            b.Entity<Customer>().HasData(
                new Customer { Id = 1, Username = "alice", Phone = "555-111-2222" },
                new Customer { Id = 2, Username = "bob", Phone = "555-222-3333" }
            );
        }
    }
}
