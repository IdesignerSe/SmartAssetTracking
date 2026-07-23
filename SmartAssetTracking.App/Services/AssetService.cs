using SmartAssetTracking.Data;
using SmartAssetTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.Services
{
    public class AssetService
    {
        public void AddAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Asset Type (Laptop/Desktop/iPhone/Samsung/Nokia/Tablet): ");
            string type = Console.ReadLine();

            Console.Write("Brand: ");
            string brand = Console.ReadLine();

            Console.Write("Model Name: ");
            string model = Console.ReadLine();

            Console.Write("Purchase Date (yyyy-mm-dd): ");
            DateTime purchaseDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Purchase Price (USD): ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Serial Number: ");
            string serial = Console.ReadLine();

            var asset = new ComputerAsset(); // default, we fix type below

            if (type.ToLower() == "laptop" || type.ToLower() == "desktop")
                asset = new ComputerAsset();
            else
                asset = new MobileAsset();

            asset.AssetType = type;
            asset.Brand = brand;
            asset.ModelName = model;
            asset.PurchaseDate = purchaseDate;
            asset.PurchasePriceUSD = price;
            asset.SerialNumber = serial;
            asset.LocalPrice = price; // temporary until currency system is added
            asset.WarrantyExpiration = purchaseDate.AddYears(3);

            db.Assets.Add(asset);
            db.SaveChanges();

            Console.WriteLine("Asset added successfully!");
        }

        public void ShowAssets()
        {
            using var db = new AssetDbContext();

            var assets = db.Assets.ToList();

            Console.WriteLine("\n=== ASSET LIST ===");
            Console.WriteLine("ID | Type | Brand | Model | Purchase Date | Status");

            foreach (var a in assets)
            {
                string status = CalculateStatus(a.PurchaseDate);

                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} | {a.ModelName} | {a.PurchaseDate:yyyy-MM-dd} | {status}");
            }
        }

        public void UpdateAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Enter Asset ID to update: ");
            int id = int.Parse(Console.ReadLine());

            var asset = db.Assets.FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                return;
            }

            Console.Write("New Brand: ");
            asset.Brand = Console.ReadLine();

            Console.Write("New Model: ");
            asset.ModelName = Console.ReadLine();

            db.SaveChanges();

            Console.WriteLine("Asset updated.");
        }

        public void DeleteAsset()
        {
            using var db = new AssetDbContext();

            Console.Write("Enter Asset ID to delete: ");
            int id = int.Parse(Console.ReadLine());

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