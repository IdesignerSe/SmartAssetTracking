using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace SmartAssetTracking.App.Services
{
    public class ExportService
    {
        public void ExportMenu()
        {
            Console.WriteLine("\nExport Options:");
            Console.WriteLine("1. Export Assets to CSV");
            Console.WriteLine("2. Export Assets to JSON");
            Console.WriteLine("3. Export Assets to TXT");
            Console.Write("Choose: ");

            string input = Console.ReadLine()!;
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input.");
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
            using var db = new AssetDbContext();

            var assets = db.Assets.Include(a => a.Office).ToList();

            string path = "assets_export.csv";

            using var writer = new StreamWriter(path);

            writer.WriteLine("Id,Type,Brand,Model,Office,Country,Price,PurchaseDate");

            foreach (var a in assets)
            {
                writer.WriteLine(
                    $"{a.Id}," +
                    $"{a.AssetType}," +
                    $"{a.Brand}," +
                    $"{a.ModelName}," +
                    $"{a.Office.OfficeName}," +
                    $"{a.Office.Country}," +
                    $"{a.LocalPrice} {a.Office.Currency}," +
                    $"{a.PurchaseDate:yyyy-MM-dd}"
                );
            }

            Console.WriteLine($"CSV exported: {path}");
        }

        private void ExportJSON()
        {
            using var db = new AssetDbContext();

            var assets = db.Assets.Include(a => a.Office).ToList();

            string path = "assets_export.json";

            var json = JsonSerializer.Serialize(assets, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);

            Console.WriteLine($"JSON exported: {path}");
        }

        private void ExportTXT()
        {
            using var db = new AssetDbContext();

            var assets = db.Assets.Include(a => a.Office).ToList();

            string path = "assets_export.txt";

            using var writer = new StreamWriter(path);

            writer.WriteLine("=== ASSET EXPORT ===");

            foreach (var a in assets)
            {
                writer.WriteLine(
                    $"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName} | " +
                    $"{a.Office.OfficeName} ({a.Office.Country}) | " +
                    $"{a.LocalPrice} {a.Office.Currency} | " +
                    $"{a.PurchaseDate:yyyy-MM-dd}"
                );
            }

            Console.WriteLine($"TXT exported: {path}");
        }
    }
}