using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class DashboardService
    {
        private readonly AssetDbContext _context;

        public DashboardService(AssetDbContext context)
        {
            _context = context;
        }

        public void ShowDashboard()
        {
            Console.Clear();
            Console.WriteLine("=== DASHBOARD ===\n");

            // Total assets
            int totalAssets = _context.Assets.Count();
            Console.WriteLine($"Total Assets: {totalAssets}");

            // Total value
            decimal totalValue = _context.Assets.Sum(a => a.PurchasePrice);
            Console.WriteLine($"Total Asset Value: {totalValue:C}");

            // Average price
            decimal avgPrice = totalAssets > 0
                ? _context.Assets.Average(a => a.PurchasePrice)
                : 0;
            Console.WriteLine($"Average Asset Price: {avgPrice:C}");

            // Most expensive asset
            var mostExpensive = _context.Assets
                .OrderByDescending(a => a.PurchasePrice)
                .FirstOrDefault();

            if (mostExpensive != null)
            {
                Console.WriteLine($"Most Expensive Asset: {mostExpensive.Brand} {mostExpensive.ModelName} ({mostExpensive.PurchasePrice:C})");
            }

            // Cheapest asset
            var cheapest = _context.Assets
                .OrderBy(a => a.PurchasePrice)
                .FirstOrDefault();

            if (cheapest != null)
            {
                Console.WriteLine($"Cheapest Asset: {cheapest.Brand} {cheapest.ModelName} ({cheapest.PurchasePrice:C})");
            }

            // Most common asset type
            var commonType = _context.Assets
                .GroupBy(a => a.AssetType)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .FirstOrDefault();

            if (commonType != null)
            {
                Console.WriteLine($"Most Common Asset Type: {commonType.Type} ({commonType.Count} items)");
            }

            Console.WriteLine("\n--- Assets Per Office ---");

            var assetsPerOffice = _context.Assets
                .Include(a => a.Office)
                .GroupBy(a => a.Office.OfficeName)
                .Select(g => new { Office = g.Key, Count = g.Count() })
                .ToList();

            foreach (var o in assetsPerOffice)
            {
                Console.WriteLine($"{o.Office}: {o.Count} assets");
            }

            Console.WriteLine("\n--- Assets Per Employee ---");

            var assetsPerEmployee = _context.Assets
                .Include(a => a.Employee)
                .Where(a => a.Employee != null)
                .GroupBy(a => a.Employee!.FullName)
                .Select(g => new { Employee = g.Key, Count = g.Count() })
                .ToList();

            if (assetsPerEmployee.Count == 0)
            {
                Console.WriteLine("No assets assigned to employees.");
            }
            else
            {
                foreach (var e in assetsPerEmployee)
                {
                    Console.WriteLine($"{e.Employee}: {e.Count} assets");
                }
            }

            Console.WriteLine("\n--- Upcoming Maintenance (Next 30 Days) ---");

            var upcomingMaintenance = _context.MaintenanceRecords
                .Include(m => m.Asset)
                .Where(m => m.NextMaintenanceDate <= DateTime.Now.AddDays(30))
                .OrderBy(m => m.NextMaintenanceDate)
                .ToList();

            if (upcomingMaintenance.Count == 0)
            {
                Console.WriteLine("No upcoming maintenance.");
            }
            else
            {
                foreach (var m in upcomingMaintenance)
                {
                    Console.WriteLine($"{m.Asset.Brand} {m.Asset.ModelName} → {m.NextMaintenanceDate:yyyy-MM-dd} | {m.Notes}");
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }
    }
}