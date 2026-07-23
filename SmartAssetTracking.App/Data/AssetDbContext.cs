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
            // Inheritance mapping
            modelBuilder.Entity<ComputerAsset>().HasBaseType<Asset>();
            modelBuilder.Entity<MobileAsset>().HasBaseType<Asset>();

            // Office → Assets (1-to-many)
            modelBuilder.Entity<Office>()
                .HasMany(o => o.Assets)
                .WithOne(a => a.Office)
                .HasForeignKey(a => a.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee → Assets (1-to-many)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.AssignedAssets)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            // MaintenanceRecord → Asset (1-to-many)
            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Asset)
                .WithMany()
                .HasForeignKey(m => m.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}