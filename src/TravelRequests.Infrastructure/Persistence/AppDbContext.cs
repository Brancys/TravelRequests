using Microsoft.EntityFrameworkCore;
using TravelRequests.Domain.Entities;

namespace TravelRequests.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TravelRequest> TravelRequests => Set<TravelRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relaciones
        modelBuilder.Entity<User>()
            .HasMany(u => u.TravelRequests)
            .WithOne(r => r.User!)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enums como string (si prefieres string en DB)
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<TravelRequest>()
            .Property(r => r.Status)
            .HasConversion<string>();
    }
}
