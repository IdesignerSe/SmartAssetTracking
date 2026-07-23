using SmartAssetTracking.Services;

class Program
{
    static void Main()
    {
        Console.Title = "Smart Asset Tracking System";

        var assetService = new AssetService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SMART ASSET TRACKING ===");
            Console.WriteLine("1. Add Asset");
            Console.WriteLine("2. Show All Assets");
            Console.WriteLine("3. Update Asset");
            Console.WriteLine("4. Delete Asset");
            Console.WriteLine("5. Exit");
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
