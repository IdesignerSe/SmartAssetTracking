using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class AssetService
    {
        private readonly AssetDbContext _context;

        public AssetService(AssetDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // CREATE (UPDATED FOR INHERITANCE)
        // ============================================================
        public void AddAsset()
        {
            Console.Clear();
            Console.WriteLine("=== ADD ASSET ===");
            Console.WriteLine("Choose asset type:");
            Console.WriteLine("1. Laptop");
            Console.WriteLine("2. Desktop");
            Console.WriteLine("3. iPhone");
            Console.WriteLine("4. Samsung");
            Console.WriteLine("5. Nokia");
            Console.WriteLine("6. Tablet");
            Console.Write("Your choice: ");

            var choice = Console.ReadLine();

            Asset asset = choice switch
            {
                "1" => new Laptop(),
                "2" => new Desktop(),
                "3" => new iPhone(),
                "4" => new Samsung(),
                "5" => new Nokia(),
                "6" => new Tablet(),
                _ => throw new Exception("Invalid choice")
            };

            // Basic info
            Console.Write("Model Name: ");
            asset.ModelName = Console.ReadLine() ?? "";

            Console.Write("Serial Number: ");
            asset.SerialNumber = Console.ReadLine() ?? "";

            Console.Write("Purchase Date (yyyy-mm-dd): ");
            asset.PurchaseDate = DateTime.Parse(Console.ReadLine() ?? "");

            Console.Write("Purchase Price (USD): ");
            asset.PurchasePrice = decimal.Parse(Console.ReadLine() ?? "");

            // PDF Level 3 fields
            asset.PurchasePriceUSD = asset.PurchasePrice;

            Console.Write("Local Price (converted): ");
            asset.LocalPrice = decimal.Parse(Console.ReadLine() ?? "");

            Console.Write("Warranty Expiration (yyyy-mm-dd): ");
            asset.WarrantyExpiration = DateTime.Parse(Console.ReadLine() ?? "");

            // Office relation
            Console.Write("Office ID: ");
            asset.OfficeId = int.Parse(Console.ReadLine() ?? "");

            // Employee assignment (optional)
            Console.Write("Assign to employee? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.Write("Employee ID: ");
                asset.EmployeeId = int.Parse(Console.ReadLine() ?? "");
            }

            // Extra fields for ComputerAsset
            if (asset is ComputerAsset comp)
            {
                Console.Write("CPU: ");
                comp.CPU = Console.ReadLine();

                Console.Write("RAM: ");
                comp.RAM = Console.ReadLine();

                Console.Write("Storage: ");
                comp.Storage = Console.ReadLine();

                Console.Write("GPU: ");
                comp.GPU = Console.ReadLine();
            }

            // Extra fields for MobileAsset
            if (asset is MobileAsset mob)
            {
                Console.Write("Operating System: ");
                mob.OperatingSystem = Console.ReadLine();

                Console.Write("Screen Size: ");
                mob.ScreenSize = Console.ReadLine();

                Console.Write("Battery Capacity: ");
                mob.BatteryCapacity = Console.ReadLine();
            }

            _context.Assets.Add(asset);
            _context.SaveChanges();

            Console.WriteLine("Asset added successfully!");
            Console.ReadKey();
        }

        // ============================================================
        // READ (UPDATED FOR INHERITANCE + MAINTENANCE)
        // ============================================================
        public void ShowAssets()
        {
            Console.Clear();
            Console.WriteLine("=== ALL ASSETS ===");

            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .Include(a => a.MaintenanceRecords)
                .ToList();

            if (!assets.Any())
            {
                Console.WriteLine("No assets found.");
                Console.ReadKey();
                return;
            }

            foreach (var a in assets)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"ID: {a.Id}");
                Console.WriteLine($"Type: {a.AssetType}");
                Console.WriteLine($"Brand: {a.Brand}");
                Console.WriteLine($"Model: {a.ModelName}");
                Console.WriteLine($"Serial Number: {a.SerialNumber}");
                Console.WriteLine($"Purchase Date: {a.PurchaseDate:yyyy-MM-dd}");
                Console.WriteLine($"Purchase Price (USD): {a.PurchasePriceUSD:C}");
                Console.WriteLine($"Local Price: {a.LocalPrice:C}");
                Console.WriteLine($"Warranty Expiration: {a.WarrantyExpiration:yyyy-MM-dd}");
                Console.WriteLine($"Lifecycle Status: {a.LifecycleStatus}");
                Console.WriteLine($"Office: {a.Office?.OfficeName ?? "None"}");
                Console.WriteLine($"Employee: {a.Employee?.FullName ?? "None"}");

                // ComputerAsset fields
                if (a is ComputerAsset comp)
                {
                    Console.WriteLine("---- Computer Specs ----");
                    Console.WriteLine($"CPU: {comp.CPU}");
                    Console.WriteLine($"RAM: {comp.RAM}");
                    Console.WriteLine($"Storage: {comp.Storage}");
                    Console.WriteLine($"GPU: {comp.GPU}");
                }

                // MobileAsset fields
                if (a is MobileAsset mob)
                {
                    Console.WriteLine("---- Mobile Specs ----");
                    Console.WriteLine($"Operating System: {mob.OperatingSystem}");
                    Console.WriteLine($"Screen Size: {mob.ScreenSize}");
                    Console.WriteLine($"Battery Capacity: {mob.BatteryCapacity}");
                }

                // Maintenance records
                if (a.MaintenanceRecords.Any())
                {
                    Console.WriteLine("---- Maintenance ----");
                    foreach (var m in a.MaintenanceRecords)
                    {
                        Console.WriteLine($"{m.Date:yyyy-MM-dd} | {m.Description} | {m.Cost:C}");
                    }
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.ReadKey();
        }

        // ============================================================
        // UPDATE (UPDATED FOR INHERITANCE)
        // ============================================================
        public void UpdateAsset()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE ASSET ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Updating {asset.AssetType} ({asset.Brand} {asset.ModelName})");

            Console.Write($"Model Name ({asset.ModelName}): ");
            string? model = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(model)) asset.ModelName = model;

            Console.Write($"Serial Number ({asset.SerialNumber}): ");
            string? serial = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(serial)) asset.SerialNumber = serial;

            Console.Write($"Purchase Price USD ({asset.PurchasePriceUSD}): ");
            if (decimal.TryParse(Console.ReadLine(), out var priceUsd))
                asset.PurchasePriceUSD = priceUsd;

            Console.Write($"Local Price ({asset.LocalPrice}): ");
            if (decimal.TryParse(Console.ReadLine(), out var localPrice))
                asset.LocalPrice = localPrice;

            Console.Write($"Warranty Expiration ({asset.WarrantyExpiration:yyyy-MM-dd}): ");
            if (DateTime.TryParse(Console.ReadLine(), out var warranty))
                asset.WarrantyExpiration = warranty;

            // ComputerAsset update
            if (asset is ComputerAsset comp)
            {
                Console.Write($"CPU ({comp.CPU}): ");
                string? cpu = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(cpu)) comp.CPU = cpu;

                Console.Write($"RAM ({comp.RAM}): ");
                string? ram = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ram)) comp.RAM = ram;

                Console.Write($"Storage ({comp.Storage}): ");
                string? storage = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(storage)) comp.Storage = storage;

                Console.Write($"GPU ({comp.GPU}): ");
                string? gpu = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(gpu)) comp.GPU = gpu;
            }

            // MobileAsset update
            if (asset is MobileAsset mob)
            {
                Console.Write($"Operating System ({mob.OperatingSystem}): ");
                string? os = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(os)) mob.OperatingSystem = os;

                Console.Write($"Screen Size ({mob.ScreenSize}): ");
                string? screen = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(screen)) mob.ScreenSize = screen;

                Console.Write($"Battery Capacity ({mob.BatteryCapacity}): ");
                string? battery = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(battery)) mob.BatteryCapacity = battery;
            }

            _context.SaveChanges();

            Console.WriteLine("Asset updated!");
            Console.ReadKey();
        }

        // ============================================================
        // DELETE
        // ============================================================
        public void DeleteAsset()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE ASSET ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            _context.Assets.Remove(asset);
            _context.SaveChanges();

            Console.WriteLine("Asset deleted!");
            Console.ReadKey();
        }

        // ============================================================
        // ADD MAINTENANCE RECORD (NEW IN STEP 11)
        // ============================================================
        public void AddMaintenanceRecord()
        {
            Console.Clear();
            Console.WriteLine("=== ADD MAINTENANCE RECORD ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets
                .Include(a => a.MaintenanceRecords)
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Maintenance Description: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Maintenance Cost: ");
            decimal cost = decimal.TryParse(Console.ReadLine(), out var c) ? c : 0;

            var record = new MaintenanceRecord
            {
                Date = DateTime.Now,
                Description = description,
                Cost = cost,
                AssetId = asset.Id
            };

            asset.MaintenanceRecords.Add(record);
            _context.SaveChanges();

            Console.WriteLine("Maintenance record added!");
            Console.ReadKey();
        }

        // ============================================================
        // SHOW MAINTENANCE HISTORY (NEW IN STEP 11)
        // ============================================================
        public void ShowMaintenance()
        {
            Console.Clear();
            Console.WriteLine("=== MAINTENANCE HISTORY ===");

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets
                .Include(a => a.MaintenanceRecords)
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Maintenance for {asset.Brand} {asset.ModelName}:");

            if (!asset.MaintenanceRecords.Any())
            {
                Console.WriteLine("No maintenance records.");
                Console.ReadKey();
                return;
            }

            foreach (var m in asset.MaintenanceRecords)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Date: {m.Date:yyyy-MM-dd}");
                Console.WriteLine($"Description: {m.Description}");
                Console.WriteLine($"Cost: {m.Cost:C}");
            }

            Console.WriteLine("----------------------------------------");
            Console.ReadKey();
        }

        // ============================================================
        // ASSET REPORT (UPDATED FOR MAINTENANCE)
        // ============================================================
        public void AssetReport()
        {
            Console.Clear();
            Console.WriteLine("=== ASSET REPORT ===");

            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .Include(a => a.MaintenanceRecords)
                .ToList();

            if (!assets.Any())
            {
                Console.WriteLine("No assets found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Total Assets: {assets.Count}");
            Console.WriteLine($"Total Value (USD): {assets.Sum(a => a.PurchasePriceUSD):C}");
            Console.WriteLine($"Total Local Value: {assets.Sum(a => a.LocalPrice):C}");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine("Assets Per Office:");
            var groupedByOffice = assets
                .GroupBy(a => a.Office?.OfficeName ?? "None")
                .Select(g => new { Office = g.Key, Count = g.Count(), Value = g.Sum(a => a.PurchasePriceUSD) });

            foreach (var office in groupedByOffice)
            {
                Console.WriteLine($"Office: {office.Office} | Count: {office.Count} | Value: {office.Value:C}");
            }

            Console.WriteLine("----------------------------------------");

            Console.WriteLine("Detailed Asset List:");
            foreach (var a in assets)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName}");
                Console.WriteLine($"Serial: {a.SerialNumber}");
                Console.WriteLine($"Purchase: {a.PurchaseDate:yyyy-MM-dd}");
                Console.WriteLine($"Price USD: {a.PurchasePriceUSD:C}");
                Console.WriteLine($"Local Price: {a.LocalPrice:C}");
                Console.WriteLine($"Warranty: {a.WarrantyExpiration:yyyy-MM-dd}");
                Console.WriteLine($"Lifecycle: {a.LifecycleStatus}");
                Console.WriteLine($"Office: {a.Office?.OfficeName ?? "None"}");
                Console.WriteLine($"Employee: {a.Employee?.FullName ?? "None"}");

                if (a is ComputerAsset comp)
                {
                    Console.WriteLine("---- Computer Specs ----");
                    Console.WriteLine($"CPU: {comp.CPU}");
                    Console.WriteLine($"RAM: {comp.RAM}");
                    Console.WriteLine($"Storage: {comp.Storage}");
                    Console.WriteLine($"GPU: {comp.GPU}");
                }

                if (a is MobileAsset mob)
                {
                    Console.WriteLine("---- Mobile Specs ----");
                    Console.WriteLine($"OS: {mob.OperatingSystem}");
                    Console.WriteLine($"Screen: {mob.ScreenSize}");
                    Console.WriteLine($"Battery: {mob.BatteryCapacity}");
                }

                if (a.MaintenanceRecords.Any())
                {
                    Console.WriteLine("---- Maintenance ----");
                    foreach (var m in a.MaintenanceRecords)
                    {
                        Console.WriteLine($"{m.Date:yyyy-MM-dd} | {m.Description} | {m.Cost:C}");
                    }
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.ReadKey();
        }

        // ============================================================
        // MASS INSERT
        // ============================================================
        public void Add10Assets()
        {
            Console.Clear();
            Console.WriteLine("=== MASS INSERT: 10 ASSETS ===");

            var firstOffice = _context.Offices.FirstOrDefault();
            if (firstOffice == null)
            {
                Console.WriteLine("No offices found. Add at least one office first.");
                Console.ReadKey();
                return;
            }

            for (int i = 1; i <= 10; i++)
            {
                var asset = new Laptop
                {
                    AssetType = "Laptop",
                    Brand = $"Brand {i}",
                    ModelName = $"Model {i}",
                    PurchaseDate = DateTime.Now.AddDays(-i),
                    PurchasePrice = 5000 + (i * 100),
                    PurchasePriceUSD = 5000 + (i * 100),
                    LocalPrice = 5000 + (i * 100),
                    OfficeId = firstOffice.Id,
                    CPU = "Intel i5",
                    RAM = "8GB",
                    Storage = "256GB SSD",
                    GPU = "Integrated"
                };

                _context.Assets.Add(asset);
            }

            _context.SaveChanges();

            Console.WriteLine("10 assets added!");
            Console.ReadKey();
        }
    }
}