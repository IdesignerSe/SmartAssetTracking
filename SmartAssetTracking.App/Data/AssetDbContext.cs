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

        // Base entities
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        // Inheritance entities (Level 1 requirement)
        public DbSet<Laptop> Laptops { get; set; } = null!;
        public DbSet<Desktop> Desktops { get; set; } = null!;
        public DbSet<iPhone> iPhones { get; set; } = null!;
        public DbSet<Samsung> Samsungs { get; set; } = null!;
        public DbSet<Nokia> Nokias { get; set; } = null!;
        public DbSet<Tablet> Tablets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User seeding
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Role = UserRole.Admin
                }
            );

            // Optional: EF Core TPH configuration (recommended)
            modelBuilder.Entity<Asset>()
                .HasDiscriminator<string>("AssetCategory")
                .HasValue<Asset>("Base")
                .HasValue<ComputerAsset>("Computer")
                .HasValue<MobileAsset>("Mobile")
                .HasValue<Laptop>("Laptop")
                .HasValue<Desktop>("Desktop")
                .HasValue<iPhone>("iPhone")
                .HasValue<Samsung>("Samsung")
                .HasValue<Nokia>("Nokia")
                .HasValue<Tablet>("Tablet");
        }
    }
}