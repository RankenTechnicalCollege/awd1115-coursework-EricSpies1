using KeyboardShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeyboardShop.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Keyboard> Keyboards => Set<Keyboard>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Keyboard>()
             .HasIndex(k => k.Slug)
             .IsUnique();

            var seed = new[]
            {
                new Keyboard{ Id=1, Name="Vortex 75 Pro", ImageFile="vortex75.jpg", Brand="Vortex", SwitchType="Linear", Layout="75%", Price=169, Connectivity="Wired"},
                new Keyboard{ Id=2, Name="Keychron K2", ImageFile="k2.jpg", Brand="Keychron", SwitchType="Tactile", Layout="75%", Price=89, Connectivity="Wireless"},
                new Keyboard{ Id=3, Name="Ducky One 2 TKL", ImageFile="ducky_tkl.jpg", Brand="Ducky", SwitchType="Clicky", Layout="TKL", Price=119, Connectivity="Wired"},
                new Keyboard{ Id=4, Name="Akko 3068B", ImageFile="akko3068b.jpg", Brand="Akko", SwitchType="Linear", Layout="65%", Price=99, Connectivity="Wireless"},
                new Keyboard{ Id=5, Name="Leopold FC660M", ImageFile="fc660m.jpg", Brand="Leopold", SwitchType="Tactile", Layout="65%", Price=139, Connectivity="Wired"},
            };

            foreach (var k in seed)
            {
                k.SetSlug();
            }

            b.Entity<Keyboard>().HasData(seed);
        }
    }
}
