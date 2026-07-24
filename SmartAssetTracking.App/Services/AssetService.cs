using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class AssetService
    {
        public void AddAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Asset Type (Laptop/Desktop/iPhone/Samsung/Nokia/Tablet): ");
            string type = Console.ReadLine()!;

            Console.Write("Brand: ");
            string brand = Console.ReadLine()!;

            Console.Write("Model Name: ");
            string model = Console.ReadLine()!;

            Console.Write("Purchase Date (yyyy-mm-dd): ");
            DateTime purchaseDate = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Purchase Price (USD): ");
            decimal price = decimal.Parse(Console.ReadLine()!);

            Console.Write("Serial Number: ");
            string serial = Console.ReadLine()!;

            // Choose correct asset type
            Asset asset = (type.ToLower() == "laptop" || type.ToLower() == "desktop")
                ? new ComputerAsset()
                : new MobileAsset();

            asset.AssetType = type;
            asset.Brand = brand;
            asset.ModelName = model;
            asset.PurchaseDate = purchaseDate;
            asset.PurchasePriceUSD = price;
            asset.SerialNumber = serial;
            asset.WarrantyExpiration = purchaseDate.AddYears(3);

            // --- OFFICE SELECTION ---
            Console.WriteLine("\nAvailable Offices:");
            foreach (var o in db.Offices)
                Console.WriteLine($"{o.Id}. {o.OfficeName} ({o.Country})");

            Console.Write("Choose Office ID: ");
            int officeId = int.Parse(Console.ReadLine()!);

            var office = db.Offices.FirstOrDefault(o => o.Id == officeId);

            if (office == null)
            {
                Console.WriteLine("Invalid office. Asset not saved.");
                return;
            }

            asset.OfficeId = officeId;
            asset.Office = office;

            // Currency conversion
            asset.LocalPrice = CurrencyService.ConvertUSD(price, office.Currency);

            db.Assets.Add(asset);
            db.SaveChanges();

            Console.WriteLine("Asset added successfully!");
        }

        // ⭐ UPDATED VERSION — SHOW OFFICE INFO
        public void ShowAssets()
        {
            using var db = new AssetDbContext();

            var assets = db.Assets
                .Include(a => a.Office)                 // Load Office relation
                .OrderBy(a => a.AssetType)
                .ThenByDescending(a => a.PurchaseDate)
                .ToList();

            Console.WriteLine("\n=== COMPANY ASSETS ===");

            string currentType = "";

            foreach (var a in assets)
            {
                if (currentType != a.AssetType)
                {
                    currentType = a.AssetType;
                    Console.WriteLine($"\n--- {currentType.ToUpper()} ---");
                }

                string status = CalculateStatus(a.PurchaseDate);

                if (status == "RED")
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (status == "YELLOW")
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ResetColor();

                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} ({a.Office.Country}) | " +
                    $"{a.LocalPrice} {a.Office.Currency} | " +
                    $"{a.PurchaseDate:yyyy-MM-dd} | {status}"
                );

                Console.ResetColor();
            }
        }

        public void UpdateAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Enter Asset ID to update: ");
            int id = int.Parse(Console.ReadLine()!);

            var asset = db.Assets.FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            Console.Write("New Brand: ");
            asset.Brand = Console.ReadLine()!;

            Console.Write("New Model: ");
            asset.ModelName = Console.ReadLine()!;

            db.SaveChanges();

            Console.WriteLine("Asset updated.");
        }

        public void DeleteAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Enter Asset ID to delete: ");
            int id = int.Parse(Console.ReadLine()!);

            var asset = db.Assets.FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            db.Assets.Remove(asset);
            db.SaveChanges();

            Console.WriteLine("Asset deleted.");
        }

        private string CalculateStatus(DateTime purchaseDate)
        {
            var lifetime = purchaseDate.AddYears(3);
            var remaining = lifetime - DateTime.Now;

            if (remaining.TotalDays < 90)
                return "YELLOW";
            if (remaining.TotalDays < 180)
                return "RED";

            return "NORMAL";
        }
    }
}