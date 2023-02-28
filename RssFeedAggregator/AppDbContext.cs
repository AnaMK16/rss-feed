using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.Models;

namespace RssFeedAggregator;

public class AppDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>()
            .HasMany(s => s.Tags)
            .WithMany(c => c.Articles)
            .UsingEntity(j => j.ToTable("ArticleTag"));
        base.OnModelCreating(modelBuilder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        // other configuration code
    }
}    