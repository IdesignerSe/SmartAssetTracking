using Microsoft.EntityFrameworkCore;
using SmartAssetTracking.App.Models;

namespace SmartAssetTracking.App.Data
{
    public class AssetDbContext : DbContext
    {
        public AssetDbContext(DbContextOptions<AssetDbContext> options)
            : base(options)
        {
        }

        // ✔ Alla DbSets är korrekta
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✔ Seeding är korrekt
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Role = UserRole.Admin
                }
            );
        }
    }
}