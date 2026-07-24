using SmartAssetTracking.App.Services;

namespace SmartAssetTracking.App
{
    public class Menu
    {
        private readonly AssetService _assetService;
        private readonly OfficeService _officeService;
        private readonly EmployeeService _employeeService;

        public Menu(AssetService assetService, OfficeService officeService, EmployeeService employeeService)
        {
            _assetService = assetService;
            _officeService = officeService;
            _employeeService = employeeService;
        }

        // ============================
        // MAIN MENU
        // ============================
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SMART ASSET TRACKING ===");
                Console.WriteLine("1. Assets");
                Console.WriteLine("2. Offices");
                Console.WriteLine("3. Employees");
                Console.WriteLine("4. Exit");
                Console.Write("Choose: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AssetMenu();
                        break;
                    case "2":
                        OfficeMenu();
                        break;
                    case "3":
                        EmployeeMenu();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ============================
        // ASSET MENU
        // ============================
        private void AssetMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ASSET MENU ===");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Asset Report");
                Console.WriteLine("6. Add Maintenance Record");
                Console.WriteLine("7. Show Maintenance");
                Console.WriteLine("8. Back");
                Console.Write("Choose: ");

                switch (Console.ReadLine())
                {
                    case "1": _assetService.AddAsset(); break;
                    case "2": _assetService.ShowAssets(); break;
                    case "3": _assetService.UpdateAsset(); break;
                    case "4": _assetService.DeleteAsset(); break;
                    case "5": _assetService.AssetReport(); break;
                    case "6": _assetService.AddMaintenanceRecord(); break;
                    case "7": _assetService.ShowMaintenance(); break;
                    case "8": return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ============================
        // OFFICE MENU
        // ============================
        private void OfficeMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== OFFICE MENU ===");
                Console.WriteLine("1. Add Office");
                Console.WriteLine("2. Show Offices");
                Console.WriteLine("3. Show Office Details");
                Console.WriteLine("4. Update Office");
                Console.WriteLine("5. Delete Office");
                Console.WriteLine("6. Assign Asset to Office");
                Console.WriteLine("7. Office Report");
                Console.WriteLine("8. Back");
                Console.Write("Choose: ");

                switch (Console.ReadLine())
                {
                    case "1": _officeService.AddOffice(); break;
                    case "2": _officeService.ShowOffices(); break;
                    case "3": _officeService.ShowOfficeDetails(); break;
                    case "4": _officeService.UpdateOffice(); break;
                    case "5": _officeService.DeleteOffice(); break;
                    case "6": _officeService.AssignAssetToOffice(); break;
                    case "7": _officeService.OfficeReport(); break;
                    case "8": return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ============================
        // EMPLOYEE MENU
        // ============================
        private void EmployeeMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== EMPLOYEE MENU ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Show Employees");
                Console.WriteLine("3. Show Employee Details");
                Console.WriteLine("4. Update Employee");
                Console.WriteLine("5. Delete Employee");
                Console.WriteLine("6. Assign Asset to Employee");
                Console.WriteLine("7. Back");
                Console.Write("Choose: ");

                switch (Console.ReadLine())
                {
                    case "1": _employeeService.AddEmployee(); break;
                    case "2": _employeeService.ShowEmployees(); break;
                    case "3": _employeeService.ShowEmployeeDetails(); break;
                    case "4": _employeeService.UpdateEmployee(); break;
                    case "5": _employeeService.DeleteEmployee(); break;
                    case "6": _employeeService.AssignAssetToEmployee(); break;
                    case "7": return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}