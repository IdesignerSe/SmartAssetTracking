using SmartAssetTracking.App.Services;
using SmartAssetTracking.App.Data;

namespace SmartAssetTracking.App
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Smart Asset Tracking System";

            var assetService = new AssetService();
            var officeService = new OfficeService();   // <-- NYTT

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SMART ASSET TRACKING ===");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show All Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Add Office");              // <-- NYTT
                Console.WriteLine("6. Show Offices");            // <-- NYTT
                Console.WriteLine("7. Assign Asset to Office");  // <-- NYTT
                Console.WriteLine("8. Office Report");           // <-- NYTT
                Console.WriteLine("9. Exit");
                Console.Write("Choose option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        assetService.AddAsset();
                        break;
                    case "2":
                        assetService.ShowAssets();
                        break;
                    case "3":
                        assetService.UpdateAsset();
                        break;
                    case "4":
                        assetService.DeleteAsset();
                        break;

                    case "5":
                        officeService.AddOffice();
                        break;
                    case "6":
                        officeService.ShowOffices();
                        break;
                    case "7":
                        officeService.AssignAssetToOffice();
                        break;
                    case "8":
                        officeService.OfficeReport();
                        break;

                    case "9":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("\nPress ENTER to continue...");
                Console.ReadLine();
            }
        }
    }
}