using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class OfficeService
    {
        public void AddOffice()
        {
            using var db = new AssetDbContext();

            Console.Write("Office Name: ");
            string name = Console.ReadLine()!;

            Console.Write("Country: ");
            string country = Console.ReadLine()!;

            Console.Write("Currency (USD/EUR/SEK/TRY): ");
            string currency = Console.ReadLine()!;

            var office = new Office
            {
                OfficeName = name,
                Country = country,
                Currency = currency
            };

            db.Offices.Add(office);
            db.SaveChanges();

            Console.WriteLine("Office added successfully!");
        }

        public void ShowOffices()
        {
            using var db = new AssetDbContext();
            var offices = db.Offices.Include(o => o.Assets).ToList();

            Console.WriteLine("\n=== COMPANY OFFICES ===");

            foreach (var o in offices)
            {
                Console.WriteLine($"{o.Id}. {o.OfficeName} ({o.Country}) - {o.Currency}");
                Console.WriteLine($"Assets: {o.Assets.Count}");
            }
        }

        public void AssignAssetToOffice()
        {
            using var db = new AssetDbContext();

            Console.Write("Asset ID: ");
            int assetId = int.Parse(Console.ReadLine()!);

            Console.Write("Office ID: ");
            int officeId = int.Parse(Console.ReadLine()!);

            var asset = db.Assets.FirstOrDefault(a => a.Id == assetId);
            var office = db.Offices.FirstOrDefault(o => o.Id == officeId);

            if (asset == null || office == null)
            {
                Console.WriteLine("Invalid asset or office.");
                return;
            }

            asset.OfficeId = officeId;
            asset.LocalPrice = CurrencyService.ConvertUSD(asset.PurchasePriceUSD, office.Currency);

            db.SaveChanges();

            Console.WriteLine("Asset assigned to office.");
        }

        public void OfficeReport()
        {
            using var db = new AssetDbContext();

            Console.Write("Office ID: ");
            int officeId = int.Parse(Console.ReadLine()!);

            var office = db.Offices
                .Include(o => o.Assets)
                .FirstOrDefault(o => o.Id == officeId);

            if (office == null)
            {
                Console.WriteLine("Office not found.");
                return;
            }

            Console.WriteLine($"\n=== {office.OfficeName.ToUpper()} OFFICE ===");

            decimal totalValue = office.Assets.Sum(a => a.LocalPrice);

            foreach (var a in office.Assets)
            {
                string status = CalculateStatus(a.PurchaseDate);

                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} | {a.LocalPrice} {office.Currency} | {status}");
            }

            Console.WriteLine($"\nTotal Office Value: {totalValue} {office.Currency}");
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