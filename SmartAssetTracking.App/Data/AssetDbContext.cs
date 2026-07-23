using Microsoft.EntityFrameworkCore;
using SmartAssetTracking.App.Models;

namespace SmartAssetTracking.App.Data
{
    public class AssetDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // SQLite database file
            options.UseSqlite("Data Source=assets.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: configure inheritance mapping
            modelBuilder.Entity<ComputerAsset>().HasBaseType<Asset>();
            modelBuilder.Entity<MobileAsset>().HasBaseType<Asset>();

            base.OnModelCreating(modelBuilder);
        }
    }
}