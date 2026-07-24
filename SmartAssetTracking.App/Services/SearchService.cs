using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class SearchService
    {
        public void SearchAssets()
        {
            using var db = new AssetDbContext();

            Console.WriteLine("\nSearch by:");
            Console.WriteLine("1. Brand");
            Console.WriteLine("2. Model");
            Console.WriteLine("3. Serial Number");
            Console.WriteLine("4. Office");
            Console.Write("Choose: ");

            string input = Console.ReadLine()!;
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            Console.Write("Enter search term: ");
            string term = Console.ReadLine()!.ToLower();

            var query = db.Assets.Include(a => a.Office).AsQueryable();

            switch (choice)
            {
                case 1:
                    query = query.Where(a => a.Brand.ToLower().Contains(term));
                    break;

                case 2:
                    query = query.Where(a => a.ModelName.ToLower().Contains(term));
                    break;

                case 3:
                    query = query.Where(a => a.SerialNumber.ToLower().Contains(term));
                    break;

                case 4:
                    query = query.Where(a => a.Office.OfficeName.ToLower().Contains(term));
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            var results = query.ToList();

            if (!results.Any())
            {
                Console.WriteLine("No results found.");
                return;
            }

            Console.WriteLine("\n=== SEARCH RESULTS ===");
            foreach (var a in results)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} | {a.LocalPrice} {a.Office.Currency}"
                );
            }
        }

        public void FilterAssets()
        {
            using var db = new AssetDbContext();

            Console.WriteLine("\nFilter by:");
            Console.WriteLine("1. Status (RED/YELLOW/NORMAL)");
            Console.WriteLine("2. Office");
            Console.WriteLine("3. Price Range");
            Console.WriteLine("4. Purchase Year");
            Console.Write("Choose: ");

            string input = Console.ReadLine()!;
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var query = db.Assets.Include(a => a.Office).AsQueryable();
            List<Asset> results = new List<Asset>();

            switch (choice)
            {
                case 1:
                    Console.Write("Enter status (RED/YELLOW/NORMAL): ");
                    string status = Console.ReadLine()!.ToUpper();

                    // EF cannot translate CalculateStatus → do client-side filtering
                    var allAssets = query.ToList();

                    results = allAssets
                        .Where(a => CalculateStatus(a.PurchaseDate) == status)
                        .ToList();
                    break;

                case 2:
                    Console.Write("Enter office name: ");
                    string office = Console.ReadLine()!.ToLower();

                    results = query
                        .Where(a => a.Office.OfficeName.ToLower().Contains(office))
                        .ToList();
                    break;

                case 3:
                    Console.Write("Min price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal min))
                    {
                        Console.WriteLine("Invalid number.");
                        return;
                    }

                    Console.Write("Max price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal max))
                    {
                        Console.WriteLine("Invalid number.");
                        return;
                    }

                    results = query
                        .Where(a => a.LocalPrice >= min && a.LocalPrice <= max)
                        .ToList();
                    break;

                case 4:
                    Console.Write("Enter year: ");
                    if (!int.TryParse(Console.ReadLine(), out int year))
                    {
                        Console.WriteLine("Invalid year.");
                        return;
                    }

                    results = query
                        .Where(a => a.PurchaseDate.Year == year)
                        .ToList();
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            if (!results.Any())
            {
                Console.WriteLine("No results found.");
                return;
            }

            Console.WriteLine("\n=== FILTER RESULTS ===");
            foreach (var a in results)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} | {a.LocalPrice} {a.Office.Currency}"
                );
            }
        }

        public void SortAssets()
        {
            using var db = new AssetDbContext();

            Console.WriteLine("\nSort by:");
            Console.WriteLine("1. Price (Low → High)");
            Console.WriteLine("2. Price (High → Low)");
            Console.WriteLine("3. Purchase Date (Newest)");
            Console.WriteLine("4. Purchase Date (Oldest)");
            Console.WriteLine("5. Brand");
            Console.WriteLine("6. Office");
            Console.Write("Choose: ");

            string input = Console.ReadLine()!;
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var query = db.Assets.Include(a => a.Office).AsQueryable();

            switch (choice)
            {
                case 1:
                    query = query.OrderBy(a => a.LocalPrice);
                    break;

                case 2:
                    query = query.OrderByDescending(a => a.LocalPrice);
                    break;

                case 3:
                    query = query.OrderByDescending(a => a.PurchaseDate);
                    break;

                case 4:
                    query = query.OrderBy(a => a.PurchaseDate);
                    break;

                case 5:
                    query = query.OrderBy(a => a.Brand);
                    break;

                case 6:
                    query = query.OrderBy(a => a.Office.OfficeName);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            var results = query.ToList();

            Console.WriteLine("\n=== SORTED ASSETS ===");
            foreach (var a in results)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} | {a.LocalPrice} {a.Office.Currency}"
                );
            }
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