using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace SmartAssetTracking.App.Services
{
    public class ExportService
    {
        private readonly AssetDbContext _context;
        private readonly string _exportFolder;

        public ExportService(AssetDbContext context)
        {
            _context = context;

            // ⭐ Export folder in project root
            _exportFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

            if (!Directory.Exists(_exportFolder))
                Directory.CreateDirectory(_exportFolder);
        }

        public void ExportMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT OPTIONS ===");
            Console.WriteLine("1. Export Assets");
            Console.WriteLine("2. Export Employees");
            Console.WriteLine("3. Export Offices");
            Console.WriteLine("4. Export Maintenance");
            Console.WriteLine("5. Back");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }

            switch (choice)
            {
                case 1: ExportAssetsMenu(); break;
                case 2: ExportEmployeesMenu(); break;
                case 3: ExportOfficesMenu(); break;
                case 4: ExportMaintenanceMenu(); break;
                case 5: return;
            }
        }

        // ============================
        // SUB-MENUS
        // ============================

        private void ExportAssetsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT ASSETS ===");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. TXT");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;

            switch (choice)
            {
                case 1: ExportAssetsCSV(); break;
                case 2: ExportAssetsJSON(); break;
                case 3: ExportAssetsTXT(); break;
            }
        }

        private void ExportEmployeesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT EMPLOYEES ===");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. TXT");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;

            switch (choice)
            {
                case 1: ExportEmployeesCSV(); break;
                case 2: ExportEmployeesJSON(); break;
                case 3: ExportEmployeesTXT(); break;
            }
        }

        private void ExportOfficesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT OFFICES ===");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. TXT");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;

            switch (choice)
            {
                case 1: ExportOfficesCSV(); break;
                case 2: ExportOfficesJSON(); break;
                case 3: ExportOfficesTXT(); break;
            }
        }

        private void ExportMaintenanceMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT MAINTENANCE ===");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. TXT");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;

            switch (choice)
            {
                case 1: ExportMaintenanceCSV(); break;
                case 2: ExportMaintenanceJSON(); break;
                case 3: ExportMaintenanceTXT(); break;
            }
        }

        // ============================
        // ASSET EXPORT
        // ============================

        private void ExportAssetsCSV()
        {
            var assets = _context.Assets.Include(a => a.Office).Include(a => a.Employee).ToList();
            if (!assets.Any()) { Console.WriteLine("No assets to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "assets_export.csv");
            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,Type,Brand,Model,Office,Country,Price,PurchaseDate,Employee");

            foreach (var a in assets)
            {
                writer.WriteLine($"{a.Id},{a.AssetType},{a.Brand},{a.ModelName}," +
                                 $"{a.Office?.OfficeName ?? "None"},{a.Office?.Country ?? "None"}," +
                                 $"{a.PurchasePrice},{a.PurchaseDate:yyyy-MM-dd}," +
                                 $"{a.Employee?.FullName ?? "None"}");
            }

            Console.WriteLine($"CSV exported: {path}");
            Console.ReadKey();
        }

        private void ExportAssetsJSON()
        {
            var assets = _context.Assets.Include(a => a.Office).Include(a => a.Employee).ToList();
            if (!assets.Any()) { Console.WriteLine("No assets to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "assets_export.json");
            File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(
                assets,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            ));

            Console.WriteLine($"JSON exported: {path}");
            Console.ReadKey();
        }

        private void ExportAssetsTXT()
        {
            var assets = _context.Assets.Include(a => a.Office).Include(a => a.Employee).ToList();
            if (!assets.Any()) { Console.WriteLine("No assets to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "assets_export.txt");
            using var writer = new StreamWriter(path);

            writer.WriteLine("=== ASSET EXPORT ===");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (var a in assets)
            {
                writer.WriteLine(
                    $"ID: {a.Id}\nType: {a.AssetType}\nBrand: {a.Brand}\nModel: {a.ModelName}\n" +
                    $"Office: {a.Office?.OfficeName ?? "None"} ({a.Office?.Country ?? "None"})\n" +
                    $"Price: {a.PurchasePrice:C}\nEmployee: {a.Employee?.FullName ?? "None"}\n" +
                    $"Purchase Date: {a.PurchaseDate:yyyy-MM-dd}\n----------------------------------------"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
            Console.ReadKey();
        }

        // ============================
        // EMPLOYEE EXPORT
        // ============================

        private void ExportEmployeesCSV()
        {
            var employees = _context.Employees.Include(e => e.AssignedAssets).ToList();
            if (!employees.Any()) { Console.WriteLine("No employees to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "employees_export.csv");
            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,FullName,Department,Email,AssetCount");

            foreach (var e in employees)
            {
                writer.WriteLine($"{e.Id},{e.FullName},{e.Department},{e.Email},{e.AssignedAssets.Count}");
            }

            Console.WriteLine($"CSV exported: {path}");
            Console.ReadKey();
        }

        private void ExportEmployeesJSON()
        {
            var employees = _context.Employees.Include(e => e.AssignedAssets).ToList();
            if (!employees.Any()) { Console.WriteLine("No employees to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "employees_export.json");
            File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(
                employees,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            ));

            Console.WriteLine($"JSON exported: {path}");
            Console.ReadKey();
        }

        private void ExportEmployeesTXT()
        {
            var employees = _context.Employees.Include(e => e.AssignedAssets).ToList();
            if (!employees.Any()) { Console.WriteLine("No employees to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "employees_export.txt");
            using var writer = new StreamWriter(path);

            writer.WriteLine("=== EMPLOYEE EXPORT ===");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (var e in employees)
            {
                writer.WriteLine(
                    $"ID: {e.Id}\nName: {e.FullName}\nDepartment: {e.Department}\nEmail: {e.Email}\n" +
                    $"Assets Assigned: {e.AssignedAssets.Count}\n----------------------------------------"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
            Console.ReadKey();
        }

        // ============================
        // OFFICE EXPORT
        // ============================

        private void ExportOfficesCSV()
        {
            var offices = _context.Offices.Include(o => o.Assets).ToList();
            if (!offices.Any()) { Console.WriteLine("No offices to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "offices_export.csv");
            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,OfficeName,Country,AssetCount");

            foreach (var o in offices)
            {
                writer.WriteLine($"{o.Id},{o.OfficeName},{o.Country},{o.Assets.Count}");
            }

            Console.WriteLine($"CSV exported: {path}");
            Console.ReadKey();
        }

        private void ExportOfficesJSON()
        {
            var offices = _context.Offices.Include(o => o.Assets).ToList();
            if (!offices.Any()) { Console.WriteLine("No offices to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "offices_export.json");
            File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(
                offices,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            ));

            Console.WriteLine($"JSON exported: {path}");
            Console.ReadKey();
        }

        private void ExportOfficesTXT()
        {
            var offices = _context.Offices.Include(o => o.Assets).ToList();
            if (!offices.Any()) { Console.WriteLine("No offices to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "offices_export.txt");
            using var writer = new StreamWriter(path);

            writer.WriteLine("=== OFFICE EXPORT ===");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (var o in offices)
            {
                writer.WriteLine(
                    $"ID: {o.Id}\nOffice: {o.OfficeName}\nCountry: {o.Country}\n" +
                    $"Assets: {o.Assets.Count}\n----------------------------------------"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
            Console.ReadKey();
        }

        // ============================
        // MAINTENANCE EXPORT
        // ============================

        private void ExportMaintenanceCSV()
        {
            var records = _context.MaintenanceRecords.Include(r => r.Asset).ToList();
            if (!records.Any()) { Console.WriteLine("No maintenance records to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "maintenance_export.csv");
            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,Asset,Description,Cost,Date");

            foreach (var r in records)
            {
                writer.WriteLine($"{r.Id},{r.Asset.Brand} {r.Asset.ModelName},{r.Description},{r.Cost},{r.Date:yyyy-MM-dd}");
            }

            Console.WriteLine($"CSV exported: {path}");
            Console.ReadKey();
        }

        private void ExportMaintenanceJSON()
        {
            var records = _context.MaintenanceRecords.Include(r => r.Asset).ToList();
            if (!records.Any()) { Console.WriteLine("No maintenance records to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "maintenance_export.json");
            File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(
                records,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            ));

            Console.WriteLine($"JSON exported: {path}");
            Console.ReadKey();
        }

        private void ExportMaintenanceTXT()
        {
            var records = _context.MaintenanceRecords.Include(r => r.Asset).ToList();
            if (!records.Any()) { Console.WriteLine("No maintenance records to export."); Console.ReadKey(); return; }

            string path = Path.Combine(_exportFolder, "maintenance_export.txt");
            using var writer = new StreamWriter(path);

            writer.WriteLine("=== MAINTENANCE EXPORT ===");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine("----------------------------------------");

            foreach (var r in records)
            {
                writer.WriteLine(
                    $"ID: {r.Id}\nAsset: {r.Asset.Brand} {r.Asset.ModelName}\n" +
                    $"Description: {r.Description}\nCost: {r.Cost:C}\nDate: {r.Date:yyyy-MM-dd}\n" +
                    $"----------------------------------------"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
            Console.ReadKey();
        }
    }
}