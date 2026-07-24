using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace SmartAssetTracking.App.Services
{
    public class ExportService
    {
        private readonly AssetDbContext _context;

        public ExportService(AssetDbContext context)
        {
            _context = context;
        }

        public void ExportMenu()
        {
            Console.Clear();
            Console.WriteLine("=== EXPORT OPTIONS ===");
            Console.WriteLine("1. Export Assets to CSV");
            Console.WriteLine("2. Export Assets to JSON");
            Console.WriteLine("3. Export Assets to TXT");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return;
            }

            switch (choice)
            {
                case 1:
                    ExportCSV();
                    break;

                case 2:
                    ExportJSON();
                    break;

                case 3:
                    ExportTXT();
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void ExportCSV()
        {
            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .ToList();

            string path = "assets_export.csv";

            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,Type,Brand,Model,Office,Country,Price,PurchaseDate,Employee");

            foreach (var a in assets)
            {
                writer.WriteLine(
                    $"{a.Id}," +
                    $"{a.AssetType}," +
                    $"{a.Brand}," +
                    $"{a.ModelName}," +
                    $"{a.Office?.OfficeName ?? "None"}," +
                    $"{a.Office?.Country ?? "None"}," +
                    $"{a.PurchasePrice}," +
                    $"{a.PurchaseDate:yyyy-MM-dd}," +
                    $"{a.Employee?.FullName ?? "None"}"
                );
            }

            Console.WriteLine($"CSV exported: {path}");
            Console.ReadKey();
        }

        private void ExportJSON()
        {
            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .ToList();

            string path = "assets_export.json";

            var json = System.Text.Json.JsonSerializer.Serialize(assets,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(path, json);

            Console.WriteLine($"JSON exported: {path}");
            Console.ReadKey();
        }

        private void ExportTXT()
        {
            var assets = _context.Assets
                .Include(a => a.Office)
                .Include(a => a.Employee)
                .ToList();

            string path = "assets_export.txt";

            using var writer = new StreamWriter(path);

            writer.WriteLine("=== ASSET EXPORT ===");

            foreach (var a in assets)
            {
                writer.WriteLine(
                    $"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office?.OfficeName ?? "None"} ({a.Office?.Country ?? "None"}) | " +
                    $"{a.PurchasePrice:C} | " +
                    $"{a.Employee?.FullName ?? "None"} | " +
                    $"{a.PurchaseDate:yyyy-MM-dd}"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
            Console.ReadKey();
        }
    }
}