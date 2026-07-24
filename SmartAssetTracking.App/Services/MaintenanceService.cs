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

        // UPDATE MAINTENANCE RECORD
        public void UpdateMaintenanceRecord()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE MAINTENANCE RECORD ===");

            Console.Write("Enter Maintenance Record ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var record = _context.MaintenanceRecords
                .Include(r => r.Asset)
                .FirstOrDefault(r => r.Id == id);

            if (record == null)
            {
                Console.WriteLine("Record not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAsset: {record.Asset.Brand} {record.Asset.ModelName}");
            Console.WriteLine($"Current Description: {record.Description}");
            Console.Write("New Description (leave empty to keep): ");
            string? newDesc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDesc))
                record.Description = newDesc;

            Console.WriteLine($"Current Cost: {record.Cost}");
            Console.Write("New Cost (leave empty to keep): ");
            string? costInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(costInput))
            {
                if (decimal.TryParse(costInput, out decimal newCost))
                    record.Cost = newCost;
                else
                {
                    Console.WriteLine("Invalid cost.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine($"Current Date: {record.Date:yyyy-MM-dd}");
            Console.Write("New Date (yyyy-mm-dd, leave empty to keep): ");
            string? dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (DateTime.TryParse(dateInput, out DateTime newDate))
                    record.Date = newDate;
                else
                {
                    Console.WriteLine("Invalid date.");
                    Console.ReadKey();
                    return;
                }
            }

            _context.SaveChanges();

            Console.WriteLine("Maintenance record updated!");
            Console.ReadKey();
        }

        // DELETE MAINTENANCE RECORD
        public void DeleteMaintenanceRecord()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE MAINTENANCE RECORD ===");

            Console.Write("Enter Maintenance Record ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var record = _context.MaintenanceRecords
                .Include(r => r.Asset)
                .FirstOrDefault(r => r.Id == id);

            if (record == null)
            {
                Console.WriteLine("Record not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Asset: {record.Asset.Brand} {record.Asset.ModelName}");
            Console.WriteLine($"Description: {record.Description}");
            Console.WriteLine($"Cost: {record.Cost:C}");
            Console.WriteLine($"Date: {record.Date:yyyy-MM-dd}");

            Console.Write("\nAre you sure you want to delete this record? (y/n): ");
            string confirm = Console.ReadLine()?.ToLower() ?? "";

            if (confirm != "y")
            {
                Console.WriteLine("Cancelled.");
                Console.ReadKey();
                return;
            }

            _context.MaintenanceRecords.Remove(record);
            _context.SaveChanges();

            Console.WriteLine("Maintenance record deleted!");
            Console.ReadKey();
        }

        // ============================
        // MAINTENANCE COST REPORT
        // ============================
        public void MaintenanceCostReport()
        {
            Console.Clear();
            Console.WriteLine("=== MAINTENANCE COST REPORT ===");

            var records = _context.MaintenanceRecords
                .Include(r => r.Asset)
                .Include(r => r.Asset.Office)
                .ToList();

            if (!records.Any())
            {
                Console.WriteLine("No maintenance records found.");
                Console.ReadKey();
                return;
            }

            decimal totalCost = records.Sum(r => r.Cost);
            Console.WriteLine($"Total Maintenance Cost: {totalCost:C}");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine("Cost Per Asset:");
            var costPerAsset = records
                .GroupBy(r => r.Asset)
                .Select(g => new
                {
                    Asset = $"{g.Key.Brand} {g.Key.ModelName}",
                    Cost = g.Sum(r => r.Cost)
                });

            foreach (var a in costPerAsset)
            {
                Console.WriteLine($"{a.Asset} | Total Cost: {a.Cost:C}");
            }

            Console.WriteLine("----------------------------------------");

            Console.WriteLine("Cost Per Office:");
            var costPerOffice = records
                .GroupBy(r => r.Asset.Office?.OfficeName ?? "None")
                .Select(g => new
                {
                    Office = g.Key,
                    Cost = g.Sum(r => r.Cost)
                });

            foreach (var o in costPerOffice)
            {
                Console.WriteLine($"{o.Office} | Total Cost: {o.Cost:C}");
            }

            Console.WriteLine("----------------------------------------");

            var mostExpensiveAsset = costPerAsset.OrderByDescending(a => a.Cost).First();
            Console.WriteLine($"Most Expensive Asset: {mostExpensiveAsset.Asset} ({mostExpensiveAsset.Cost:C})");

            var latest = records.OrderByDescending(r => r.Date).First();
            var oldest = records.OrderBy(r => r.Date).First();

            Console.WriteLine($"Latest Maintenance: {latest.Asset.Brand} {latest.Asset.ModelName} ({latest.Date:yyyy-MM-dd})");
            Console.WriteLine($"Oldest Maintenance: {oldest.Asset.Brand} {oldest.Asset.ModelName} ({oldest.Date:yyyy-MM-dd})");

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }
    }
}