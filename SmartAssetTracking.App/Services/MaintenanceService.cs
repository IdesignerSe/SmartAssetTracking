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
        public void AddMaintenance()
        {
            Console.Clear();
            Console.WriteLine("=== ADD MAINTENANCE RECORD ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets
                .Include(a => a.MaintenanceRecords)
                .FirstOrDefault(a => a.Id == assetId);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Last Maintenance Date (yyyy-mm-dd): ");
            DateTime lastDate = DateTime.TryParse(Console.ReadLine(), out var ld)
                ? ld
                : DateTime.Now;

            Console.Write("Next Maintenance Date (yyyy-mm-dd): ");
            DateTime nextDate = DateTime.TryParse(Console.ReadLine(), out var nd)
                ? nd
                : DateTime.Now.AddMonths(6);

            Console.Write("Notes: ");
            string notes = Console.ReadLine() ?? string.Empty;

            var record = new MaintenanceRecord
            {
                LastMaintenanceDate = lastDate,
                NextMaintenanceDate = nextDate,
                Notes = notes,
                AssetId = asset.Id
            };

            asset.MaintenanceRecords.Add(record);
            _context.SaveChanges();

            Console.WriteLine("Maintenance record added!");
            Console.ReadKey();
        }

        // SHOW MAINTENANCE HISTORY
        public void ShowMaintenance()
        {
            Console.Clear();
            Console.WriteLine("=== MAINTENANCE HISTORY ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets
                .Include(a => a.MaintenanceRecords)
                .FirstOrDefault(a => a.Id == assetId);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAsset: {asset.Brand} {asset.ModelName}");
            Console.WriteLine("Maintenance Records:");

            if (asset.MaintenanceRecords.Count == 0)
            {
                Console.WriteLine("No maintenance records found.");
            }
            else
            {
                foreach (var m in asset.MaintenanceRecords)
                {
                    Console.WriteLine(
                        $"{m.Id} | Last: {m.LastMaintenanceDate:yyyy-MM-dd} | Next: {m.NextMaintenanceDate:yyyy-MM-dd} | Notes: {m.Notes}"
                    );
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // UPCOMING MAINTENANCE (Dashboard)
        public void ShowUpcomingMaintenance()
        {
            Console.Clear();
            Console.WriteLine("=== UPCOMING MAINTENANCE ===");

            var upcoming = _context.MaintenanceRecords
                .Include(m => m.Asset)
                .Where(m => m.NextMaintenanceDate <= DateTime.Now.AddDays(30))
                .OrderBy(m => m.NextMaintenanceDate)
                .ToList();

            if (upcoming.Count == 0)
            {
                Console.WriteLine("No upcoming maintenance within 30 days.");
            }
            else
            {
                foreach (var m in upcoming)
                {
                    Console.WriteLine(
                        $"{m.Asset.Brand} {m.Asset.ModelName} → Next: {m.NextMaintenanceDate:yyyy-MM-dd} | Notes: {m.Notes}"
                    );
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }
    }
}