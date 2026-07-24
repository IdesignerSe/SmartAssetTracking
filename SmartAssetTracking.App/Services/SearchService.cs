using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class SearchService
    {
        private readonly AssetDbContext _context;

        public SearchService(AssetDbContext context)
        {
            _context = context;
        }

        // SEARCH ASSETS
        public void SearchAssets()
        {
            Console.Clear();
            Console.WriteLine("=== SEARCH ASSETS ===");
            Console.WriteLine("1. Brand");
            Console.WriteLine("2. Model");
            Console.WriteLine("3. Office");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter search term: ");
            string term = Console.ReadLine()?.ToLower() ?? "";

            var query = _context.Assets
                .Include(a => a.Office)
                .AsQueryable();

            switch (choice)
            {
                case 1:
                    query = query.Where(a => a.Brand.ToLower().Contains(term));
                    break;

                case 2:
                    query = query.Where(a => a.ModelName.ToLower().Contains(term));
                    break;

                case 3:
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
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n=== RESULTS ===");
            foreach (var a in results)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} | {a.PurchasePrice:C}"
                );
            }

            Console.ReadKey();
        }

        // FILTER ASSETS
        public void FilterAssets()
        {
            Console.Clear();
            Console.WriteLine("=== FILTER ASSETS ===");
            Console.WriteLine("1. Status (RED/YELLOW/NORMAL)");
            Console.WriteLine("2. Office");
            Console.WriteLine("3. Price Range");
            Console.WriteLine("4. Purchase Year");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }

            var query = _context.Assets
                .Include(a => a.Office)
                .AsQueryable();

            List<Asset> results = new();

            switch (choice)
            {
                case 1:
                    Console.Write("Enter status: ");
                    string status = Console.ReadLine()?.ToUpper() ?? "";

                    var allAssets = query.ToList();

                    results = allAssets
                        .Where(a => CalculateStatus(a.PurchaseDate) == status)
                        .ToList();
                    break;

                case 2:
                    Console.Write("Enter office name: ");
                    string office = Console.ReadLine()?.ToLower() ?? "";

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
                        .Where(a => a.PurchasePrice >= min && a.PurchasePrice <= max)
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
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n=== FILTER RESULTS ===");
            foreach (var a in results)
            {
                Console.WriteLine(
                    $"{a.Id} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} | {a.PurchasePrice:C}"
                );
            }

            Console.ReadKey();
        }

        // SORT ASSETS
        public void SortAssets()
        {
            Console.Clear();
            Console.WriteLine("=== SORT ASSETS ===");
            Console.WriteLine("1. Price (Low → High)");
            Console.WriteLine("2. Price (High → Low)");
            Console.WriteLine("3. Purchase Date (Newest)");
            Console.WriteLine("4. Purchase Date (Oldest)");
            Console.WriteLine("5. Brand");
            Console.WriteLine("6. Office");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }

            var query = _context.Assets
                .Include(a => a.Office)
                .AsQueryable();

            switch (choice)
            {
                case 1:
                    query = query.OrderBy(a => a.PurchasePrice);
                    break;

                case 2:
                    query = query.OrderByDescending(a => a.PurchasePrice);
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
                    $"{a.Office.OfficeName} | {a.PurchasePrice:C}"
                );
            }

            Console.ReadKey();
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