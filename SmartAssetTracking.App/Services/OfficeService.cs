using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class OfficeService
    {
        private readonly AssetDbContext _context;

        public OfficeService(AssetDbContext context)
        {
            _context = context;
        }

        // ADD OFFICE
        public void AddOffice()
        {
            Console.Clear();
            Console.WriteLine("=== ADD OFFICE ===");

            Console.Write("Office Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Country: ");
            string country = Console.ReadLine() ?? string.Empty;

            var office = new Office
            {
                OfficeName = name,
                Country = country,
                Assets = new List<Asset>() // ensure non-null
            };

            _context.Offices.Add(office);
            _context.SaveChanges();

            Console.WriteLine("Office added!");
            Console.ReadKey();
        }

        // SHOW OFFICES
        public void ShowOffices()
        {
            Console.Clear();
            Console.WriteLine("=== OFFICES ===");

            var offices = _context.Offices
                .Include(o => o.Assets)
                .ToList();

            if (!offices.Any())
            {
                Console.WriteLine("No offices found.");
                Console.ReadKey();
                return;
            }

            foreach (var o in offices)
            {
                int count = o.Assets?.Count ?? 0;
                Console.WriteLine($"{o.Id} | {o.OfficeName} ({o.Country}) | Assets: {count}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // UPDATE OFFICE
        public void UpdateOffice()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE OFFICE ===");

            Console.Write("Enter Office ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var office = _context.Offices.FirstOrDefault(o => o.Id == id);
            if (office == null)
            {
                Console.WriteLine("Office not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCurrent Name: {office.OfficeName}");
            Console.Write("New Name (leave empty to keep): ");
            string? newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                office.OfficeName = newName;

            Console.WriteLine($"Current Country: {office.Country}");
            Console.Write("New Country (leave empty to keep): ");
            string? newCountry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCountry))
                office.Country = newCountry;

            _context.SaveChanges();

            Console.WriteLine("Office updated!");
            Console.ReadKey();
        }

        // ASSIGN ASSET TO OFFICE
        public void AssignAssetToOffice()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN ASSET TO OFFICE ===");

            Console.Write("Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Office ID: ");
            if (!int.TryParse(Console.ReadLine(), out int officeId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == assetId);
            var office = _context.Offices.FirstOrDefault(o => o.Id == officeId);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            if (office == null)
            {
                Console.WriteLine("Office not found.");
                Console.ReadKey();
                return;
            }

            asset.OfficeId = officeId;
            _context.SaveChanges();

            Console.WriteLine("Asset assigned!");
            Console.ReadKey();
        }

        // OFFICE REPORT
        public void OfficeReport()
        {
            Console.Clear();
            Console.WriteLine("=== OFFICE REPORT ===");

            Console.Write("Office ID: ");
            if (!int.TryParse(Console.ReadLine(), out int officeId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var office = _context.Offices
                .Include(o => o.Assets)
                .FirstOrDefault(o => o.Id == officeId);

            if (office == null)
            {
                Console.WriteLine("Office not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nOffice: {office.OfficeName} ({office.Country})");

            foreach (var a in office.Assets)
            {
                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName} | {a.PurchasePrice:C}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // DELETE OFFICE
        public void DeleteOffice()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE OFFICE ===");

            Console.Write("Office ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var office = _context.Offices
                .Include(o => o.Assets)
                .FirstOrDefault(o => o.Id == id);

            if (office == null)
            {
                Console.WriteLine("Office not found.");
                Console.ReadKey();
                return;
            }

            if (office.Assets != null)
            {
                foreach (var asset in office.Assets)
                    _context.Assets.Remove(asset);
            }

            _context.Offices.Remove(office);
            _context.SaveChanges();

            Console.WriteLine("Office deleted!");
            Console.ReadKey();
        }

        // ============================
        // MASS INSERT: 10 OFFICES
        // ============================
        public void Add10Offices()
        {
            Console.Clear();
            Console.WriteLine("=== MASS INSERT: 10 OFFICES ===");

            for (int i = 1; i <= 10; i++)
            {
                var office = new Office
                {
                    OfficeName = $"Office {i}",
                    Country = "Sweden",
                    Assets = new List<Asset>()
                };

                _context.Offices.Add(office);
            }

            _context.SaveChanges();

            Console.WriteLine("10 offices added!");
            Console.ReadKey();
        }
    }
}