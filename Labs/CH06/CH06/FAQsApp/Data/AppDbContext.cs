using Microsoft.EntityFrameworkCore;
using FAQsApp.Models;

namespace FAQsApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Faq> Faqs => Set<Faq>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Faq>()
          .HasOne(f => f.Topic)
          .WithMany(t => t.Faqs)
          .HasForeignKey(f => f.TopicId)
          .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Faq>()
          .HasOne(f => f.Category)
          .WithMany(c => c.Faqs)
          .HasForeignKey(f => f.CategoryId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
