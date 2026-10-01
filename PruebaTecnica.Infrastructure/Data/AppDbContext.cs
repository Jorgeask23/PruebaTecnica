using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Email)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.Property(x => x.Password)
                .IsRequired();

            entity.HasMany(x => x.Addresses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Street)
                .IsRequired();

            entity.Property(x => x.City)
                .IsRequired();

            entity.Property(x => x.Country)
                .IsRequired();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .IsRequired();

            entity.HasIndex(x => x.Code)
                .IsUnique();

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.RateToBase)
                .IsRequired();
        });
    }
}