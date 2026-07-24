using SmartAssetTracking.App.Services;

namespace SmartAssetTracking.App
{
    class Program
    {
        static void Main()
        {
            var assetService = new AssetService();
            var officeService = new OfficeService();
            var searchService = new SearchService();
            var exportService = new ExportService();   // ⭐ NEW

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SMART ASSET TRACKING ===");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show All Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Add Office");
                Console.WriteLine("6. Show Offices");
                Console.WriteLine("7. Assign Asset to Office");
                Console.WriteLine("8. Office Report");
                Console.WriteLine("9. Delete Office");
                Console.WriteLine("10. Search Assets");
                Console.WriteLine("11. Filter Assets");
                Console.WriteLine("12. Sort Assets");
                Console.WriteLine("13. Export Assets");   // ⭐ NEW
                Console.WriteLine("14. Exit");            // ⭐ ALWAYS LAST
                Console.Write("Choose option: ");

                string input = Console.ReadLine()!;

                if (!int.TryParse(input, out int option))
                {
                    Console.WriteLine("Invalid input. Press ENTER...");
                    Console.ReadLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        assetService.AddAsset();
                        break;

                    case 2:
                        assetService.ShowAssets();
                        break;

                    case 3:
                        assetService.UpdateAsset();
                        break;

                    case 4:
                        assetService.DeleteAsset();
                        break;

                    case 5:
                        officeService.AddOffice();
                        break;

                    case 6:
                        officeService.ShowOffices();
                        break;

                    case 7:
                        officeService.AssignAssetToOffice();
                        break;

                    case 8:
                        officeService.OfficeReport();
                        break;

                    case 9:
                        officeService.DeleteOffice();
                        break;

                    case 10:
                        searchService.SearchAssets();
                        break;

                    case 11:
                        searchService.FilterAssets();
                        break;

                    case 12:
                        searchService.SortAssets();
                        break;

                    case 13:
                        exportService.ExportMenu();
                        break;

                    case 14:
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress ENTER to continue...");
                Console.ReadLine();
            }
        }
    }
}