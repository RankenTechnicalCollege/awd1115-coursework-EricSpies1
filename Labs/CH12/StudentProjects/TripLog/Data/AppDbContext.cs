using Microsoft.EntityFrameworkCore;
using TripLog.Models;

namespace TripLog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Destination> Destinations { get; set; } = null!;
        public DbSet<Accommodation> Accommodations { get; set; } = null!;
        public DbSet<TripLog.Models.Activity> Activities { get; set; } = null!;
        public DbSet<TripActivity> TripActivities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TripActivity>()
                .HasKey(ta => new { ta.TripId, ta.ActivityId });

            modelBuilder.Entity<TripActivity>()
                .HasOne(ta => ta.Trip)
                .WithMany(t => t.TripActivities)
                .HasForeignKey(ta => ta.TripId);

            modelBuilder.Entity<TripActivity>()
                .HasOne(ta => ta.Activity)
                .WithMany(a => a.TripActivities)
                .HasForeignKey(ta => ta.ActivityId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Destination)
                .WithMany(d => d.Trips)
                .HasForeignKey(t => t.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Accommodation)
                .WithMany(a => a.Trips)
                .HasForeignKey(t => t.AccommodationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
