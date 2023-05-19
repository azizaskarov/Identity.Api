using Identity.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.ToTable("users");
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.Email).IsRequired(false).HasMaxLength(128);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();

        });
    }
}