using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using SmartAssetTracking.App.Services;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App
{
    class Program
    {
        static void Main()
        {
            // EF Core database setup
            var options = new DbContextOptionsBuilder<AssetDbContext>()
                .UseSqlite("Data Source=assets.db")
                .Options;

            using var context = new AssetDbContext(options);
            context.Database.EnsureCreated();

            // Login system
            var loginService = new LoginService();
            User loggedInUser = loginService.Login();

            MainMenu(loggedInUser, context);
        }

        static void MainMenu(User user, AssetDbContext context)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MAIN MENU ===");
                Console.WriteLine($"Logged in as: {user.Username} ({user.Role})\n");

                switch (user.Role)
                {
                    case UserRole.Admin:
                        AdminMenu(context);
                        break;

                    case UserRole.Manager:
                        ManagerMenu(context);
                        break;

                    case UserRole.Employee:
                        EmployeeMenu(context, user);
                        break;
                }
            }
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

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN MENU ===");
                Console.WriteLine("1. Asset Management");
                Console.WriteLine("2. Employee Management");
                Console.WriteLine("3. Maintenance");
                Console.WriteLine("4. Dashboard");
                Console.WriteLine("5. Exit");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": AssetMenu(assetService); break;
                    case "2": EmployeeManagementMenu(employeeService); break;
                    case "3": MaintenanceMenu(maintenanceService); break;
                    case "4": dashboardService.ShowDashboard(); break;
                    case "5": Environment.Exit(0); break;
                }
            }
        }

        // ============================
        // MANAGER MENU
        // ============================
        static void ManagerMenu(AssetDbContext context)
        {
            var assetService = new AssetService(context);
            var employeeService = new EmployeeService(context);
            var maintenanceService = new MaintenanceService(context);
            var dashboardService = new DashboardService(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MANAGER MENU ===");
                Console.WriteLine("1. Asset Management");
                Console.WriteLine("2. Employee Management");
                Console.WriteLine("3. Maintenance");
                Console.WriteLine("4. Dashboard");
                Console.WriteLine("5. Exit");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": AssetMenu(assetService); break;
                    case "2": EmployeeManagementMenu(employeeService); break;
                    case "3": MaintenanceMenu(maintenanceService); break;
                    case "4": dashboardService.ShowDashboard(); break;
                    case "5": Environment.Exit(0); break;
                }
            }
        }

        // ============================
        // EMPLOYEE MENU
        // ============================
        static void EmployeeMenu(AssetDbContext context, User user)
        {
            var employeeService = new EmployeeService(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== EMPLOYEE MENU ===");
                Console.WriteLine("1. View My Assigned Assets");
                Console.WriteLine("2. Exit");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        employeeService.ShowEmployeeAssets();
                        break;

                    case "2":
                        Environment.Exit(0);
                        break;
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
                Console.WriteLine("=== ASSET MENU ===");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Back");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": assetService.AddAsset(); break;
                    case "2": assetService.ShowAssets(); break;
                    case "3": assetService.UpdateAsset(); break;
                    case "4": assetService.DeleteAsset(); break;
                    case "5": return;
                }
            }
        }

        // ============================
        // EMPLOYEE MANAGEMENT MENU
        // ============================
        static void EmployeeManagementMenu(EmployeeService employeeService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== EMPLOYEE MANAGEMENT ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Show Employees");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. Assign Asset to Employee");
                Console.WriteLine("6. Show Employee Assets");
                Console.WriteLine("7. Back");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": employeeService.AddEmployee(); break;
                    case "2": employeeService.ShowEmployees(); break;
                    case "3": employeeService.UpdateEmployee(); break;
                    case "4": employeeService.DeleteEmployee(); break;
                    case "5": employeeService.AssignAssetToEmployee(); break;
                    case "6": employeeService.ShowEmployeeAssets(); break;
                    case "7": return;
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
                Console.WriteLine("=== MAINTENANCE MENU ===");
                Console.WriteLine("1. Add Maintenance Record");
                Console.WriteLine("2. Show Maintenance History");
                Console.WriteLine("3. Show Upcoming Maintenance");
                Console.WriteLine("4. Back");

                Console.Write("Choose option: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": maintenanceService.AddMaintenance(); break;
                    case "2": maintenanceService.ShowMaintenance(); break;
                    case "3": maintenanceService.ShowUpcomingMaintenance(); break;
                    case "4": return;
                }
            }
        }
    }
}