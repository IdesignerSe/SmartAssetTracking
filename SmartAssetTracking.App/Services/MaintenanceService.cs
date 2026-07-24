using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class MaintenanceService
    {
        private readonly AssetDbContext _context;

        public MaintenanceService(AssetDbContext context)
        {
            _context = context;
        }

        // ADD MAINTENANCE RECORD
        public void AddMaintenanceRecord()
        {
            Console.Clear();
            Console.WriteLine("=== ADD MAINTENANCE RECORD ===");

            Console.Write("Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Description: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Cost: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cost))
            {
                Console.WriteLine("Invalid cost.");
                Console.ReadKey();
                return;
            }

            var record = new MaintenanceRecord
            {
                AssetId = assetId,
                Description = description,
                Cost = cost,
                Date = DateTime.Now
            };

            _context.MaintenanceRecords.Add(record);
            _context.SaveChanges();

            Console.WriteLine("Maintenance record added!");
            Console.ReadKey();
        }

        // SHOW MAINTENANCE RECORDS
        public void ShowMaintenanceRecords()
        {
            Console.Clear();
            Console.WriteLine("=== MAINTENANCE RECORDS ===");

            var records = _context.MaintenanceRecords
                .Include(r => r.Asset)
                .ToList();

            if (!records.Any())
            {
                Console.WriteLine("No maintenance records found.");
                Console.ReadKey();
                return;
            }

            foreach (var r in records)
            {
                Console.WriteLine(
                    $"{r.Id} | Asset: {r.Asset.Brand} {r.Asset.ModelName} | " +
                    $"{r.Description} | Cost: {r.Cost:C} | Date: {r.Date:yyyy-MM-dd}"
                );
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }
    }
}