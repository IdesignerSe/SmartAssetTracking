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

        // CREATE
        public void AddAsset()
        {
            Console.Clear();
            Console.WriteLine("=== ADD ASSET ===");

            Console.Write("Asset Type: ");
            string type = Console.ReadLine() ?? string.Empty;

            Console.Write("Brand: ");
            string brand = Console.ReadLine() ?? string.Empty;

            Console.Write("Model Name: ");
            string model = Console.ReadLine() ?? string.Empty;

            Console.Write("Purchase Date (yyyy-mm-dd): ");
            DateTime purchaseDate = DateTime.TryParse(Console.ReadLine(), out var pd)
                ? pd
                : DateTime.Now;

            Console.Write("Purchase Price: ");
            decimal price = decimal.TryParse(Console.ReadLine(), out var pr)
                ? pr
                : 0;

            Console.Write("Office ID: ");
            int officeId = int.TryParse(Console.ReadLine(), out var oid)
                ? oid
                : 0;

            var asset = new Asset
            {
                AssetType = type,
                Brand = brand,
                ModelName = model,
                PurchaseDate = purchaseDate,
                PurchasePrice = price,
                OfficeId = officeId
            };

            _context.Assets.Add(asset);
            _context.SaveChanges();

            Console.WriteLine("Asset added!");
            Console.ReadKey();
        }

        // READ
        public void ShowAssets()
        {
            Console.Clear();
            Console.WriteLine("=== ALL ASSETS ===");

            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .ToList();

            foreach (var a in assets)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName} | {a.PurchasePrice:C} | Office: {a.Office.OfficeName} | Employee: {a.Employee?.FullName ?? "None"}"
                );
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // UPDATE
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

            Console.Write($"Asset Type ({asset.AssetType}): ");
            string? type = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(type)) asset.AssetType = type;

            Console.Write($"Brand ({asset.Brand}): ");
            string? brand = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(brand)) asset.Brand = brand;

            Console.Write($"Model Name ({asset.ModelName}): ");
            string? model = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(model)) asset.ModelName = model;

            Console.Write($"Purchase Price ({asset.PurchasePrice}): ");
            if (decimal.TryParse(Console.ReadLine(), out var price))
                asset.PurchasePrice = price;

            _context.SaveChanges();

            Console.WriteLine("Asset updated!");
            Console.ReadKey();
        }

        // DELETE
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
    }
}