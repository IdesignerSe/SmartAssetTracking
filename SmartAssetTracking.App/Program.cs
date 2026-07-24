using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using SmartAssetTracking.App.Services;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App
{
    public class Program
    {
        static void Main()
        {
            // ✔ Rätt databasplats
            var options = new DbContextOptionsBuilder<AssetDbContext>()
                .UseSqlite("Data Source=Data/assets.db")
                .Options;

            using var context = new AssetDbContext(options);
            context.Database.EnsureCreated();

            var loginService = new LoginService(context);

            Console.Clear();
            Console.WriteLine("=== LOGIN ===");
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            var user = loginService.Authenticate(username, password);

            if (user == null)
            {
                Console.WriteLine("Invalid login.");
                return;
            }

            MainMenu(user, context);
        }

        static void MainMenu(User user, AssetDbContext context)
        {
            if (user.Role == UserRole.Admin)
                AdminMenu(context);
            else
                UserMenu(context);
        }

        // ============================
        // ADMIN MENU
        // ============================
        static void AdminMenu(AssetDbContext context)
        {
            var assetService = new AssetService(context);
            var employeeService = new EmployeeService(context);
            var maintenanceService = new MaintenanceService(context);
            var dashboardService = new DashboardService(context);
            var officeService = new OfficeService(context);
            var searchService = new SearchService(context);
            var exportService = new ExportService(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENU ===");
                Console.WriteLine("1. Asset Management");
                Console.WriteLine("2. Employee Management");
                Console.WriteLine("3. Maintenance");
                Console.WriteLine("4. Dashboard");
                Console.WriteLine("5. Office Management");
                Console.WriteLine("6. Exit");
                Console.WriteLine("7. Export");   // ⭐ FIX: Export synlig
                Console.WriteLine("8. Search");   // ⭐ FIX: Search synlig
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        AssetMenu(assetService);
                        break;
                    case 2:
                        EmployeeMenu(employeeService);
                        break;
                    case 3:
                        MaintenanceMenu(maintenanceService);
                        break;
                    case 4:
                        dashboardService.ShowDashboard();
                        break;
                    case 5:
                        OfficeMenu(officeService);
                        break;
                    case 6:
                        return;
                    case 7:
                        exportService.ExportMenu();
                        break;
                    case 8:
                        searchService.SearchAssets();
                        break;
                }
            }
        }

        // ============================
        // USER MENU
        // ============================
        static void UserMenu(AssetDbContext context)
        {
            var assetService = new AssetService(context);
            var officeService = new OfficeService(context);
            var searchService = new SearchService(context);
            var exportService = new ExportService(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== USER MENU ===");
                Console.WriteLine("1. Asset Management");
                Console.WriteLine("2. Office Management");
                Console.WriteLine("3. Search");
                Console.WriteLine("4. Export");
                Console.WriteLine("5. Exit");
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        AssetMenu(assetService);
                        break;
                    case 2:
                        OfficeMenu(officeService);
                        break;
                    case 3:
                        searchService.SearchAssets();
                        break;
                    case 4:
                        exportService.ExportMenu();
                        break;
                    case 5:
                        return;
                }
            }
        }

        // ============================
        // OFFICE MENU
        // ============================
        static void OfficeMenu(OfficeService officeService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== OFFICE MANAGEMENT ===");
                Console.WriteLine("1. Add Office");
                Console.WriteLine("2. Show Offices");
                Console.WriteLine("3. Assign Asset to Office");
                Console.WriteLine("4. Office Report");
                Console.WriteLine("5. Delete Office");
                Console.WriteLine("6. Back");
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        officeService.AddOffice();
                        break;
                    case 2:
                        officeService.ShowOffices();
                        break;
                    case 3:
                        officeService.AssignAssetToOffice();
                        break;
                    case 4:
                        officeService.OfficeReport();
                        break;
                    case 5:
                        officeService.DeleteOffice();
                        break;
                    case 6:
                        return;
                }
            }
        }

        // ============================
        // ASSET MENU
        // ============================
        static void AssetMenu(AssetService assetService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ASSET MANAGEMENT ===");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Back");
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
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
                        return;
                }
            }
        }

        // ============================
        // EMPLOYEE MENU
        // ============================
        static void EmployeeMenu(EmployeeService employeeService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== EMPLOYEE MANAGEMENT ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Show Employees");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. Assign Asset");
                Console.WriteLine("6. Show Employee Assets");
                Console.WriteLine("7. Back");
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        employeeService.AddEmployee();
                        break;
                    case 2:
                        employeeService.ShowEmployees();
                        break;
                    case 3:
                        employeeService.AssignAssetToEmployee();
                        break;
                    case 4:
                        employeeService.DeleteEmployee();
                        break;
                    case 5:
                        employeeService.AssignAssetToEmployee();
                        break;
                    case 6:
                        employeeService.ShowEmployeeAssets();
                        break;
                    case 7:
                        return;
                }
            }
        }

        // ============================
        // MAINTENANCE MENU
        // ============================
        static void MaintenanceMenu(MaintenanceService maintenanceService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MAINTENANCE ===");
                Console.WriteLine("1. Add Maintenance Record");
                Console.WriteLine("2. Show Maintenance Records");
                Console.WriteLine("3. Back");
                Console.Write("Choose option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        maintenanceService.AddMaintenanceRecord();
                        break;
                    case 2:
                        maintenanceService.ShowMaintenanceRecords();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}