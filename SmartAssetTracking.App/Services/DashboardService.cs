using SmartAssetTracking.App.Data;
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

            ShowAssetCount();
            ShowOfficeSummary();
            ShowEmployeeSummary();
            ShowMaintenanceSummary();

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // ============================
        // TOTAL ASSETS
        // ============================
        private void ShowAssetCount()
        {
            int totalAssets = _context.Assets.Count();
            Console.WriteLine($"Total Assets: {totalAssets}");
        }

        // ============================
        // OFFICE SUMMARY
        // ============================
        private void ShowOfficeSummary()
        {
            Console.WriteLine("\n--- Offices ---");

            var offices = _context.Offices
                .Include(o => o.Assets)
                .ToList();

            if (!offices.Any())
            {
                Console.WriteLine("No offices found.");
                return;
            }

            foreach (var office in offices)
            {
                int assetCount = office.Assets?.Count ?? 0;

                Console.WriteLine(
                    $"{office.Id}. {office.OfficeName} ({office.Country}) → {assetCount} assets"
                );
            }
        }

        // ============================
        // EMPLOYEE SUMMARY
        // ============================
        private void ShowEmployeeSummary()
        {
            Console.WriteLine("\n--- Employees ---");

            var employees = _context.Employees
                .Include(e => e.AssignedAssets)
                .ToList();

            if (!employees.Any())
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var emp in employees)
            {
                int assigned = emp.AssignedAssets.Count;

                Console.WriteLine(
                    $"{emp.Id}. {emp.FullName} ({emp.Department}) → {assigned} assets"
                );
            }
        }

        // ============================
        // MAINTENANCE SUMMARY
        // ============================
        private void ShowMaintenanceSummary()
        {
            Console.WriteLine("\n--- Maintenance ---");

            var records = _context.MaintenanceRecords
                .Include(r => r.Asset)
                .ToList();

            if (!records.Any())
            {
                Console.WriteLine("No maintenance records found.");
                return;
            }

            decimal totalCost = records.Sum(r => r.Cost);
            DateTime lastMaintenance = records.Max(r => r.Date);

            Console.WriteLine($"Total Maintenance Cost: {totalCost:C}");
            Console.WriteLine($"Last Maintenance Date: {lastMaintenance:yyyy-MM-dd}");

            Console.WriteLine("\nRecent Records:");
            foreach (var r in records.OrderByDescending(r => r.Date).Take(5))
            {
                Console.WriteLine(
                    $"{r.Id} | {r.Asset.Brand} {r.Asset.ModelName} | " +
                    $"{r.Description} | Cost: {r.Cost:C} | Date: {r.Date:yyyy-MM-dd}"
                );
            }
        }
    }
}