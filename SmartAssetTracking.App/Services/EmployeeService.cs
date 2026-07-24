using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartAssetTracking.App.Services
{
    public class EmployeeService
    {
        private readonly AssetDbContext _context;

        public EmployeeService(AssetDbContext context)
        {
            _context = context;
        }

        // CREATE
        public void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== ADD EMPLOYEE ===");

            Console.Write("Full Name: ");
            string fullName = Console.ReadLine() ?? string.Empty;

            Console.Write("Department: ");
            string department = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            var employee = new Employee
            {
                FullName = fullName,
                Department = department,
                Email = email
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            Console.WriteLine("Employee added!");
            Console.ReadKey();
        }

        // READ
        public void ShowEmployees()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEES ===");

            var employees = _context.Employees.ToList();

            if (!employees.Any())
            {
                Console.WriteLine("No employees found.");
                Console.ReadKey();
                return;
            }

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.Id} | {e.FullName} | {e.Department} | {e.Email}");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }

        // UPDATE
        public void UpdateEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE EMPLOYEE ===");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Full Name ({employee.FullName}): ");
            string? fullName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fullName))
                employee.FullName = fullName;

            Console.Write($"Department ({employee.Department}): ");
            string? department = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(department))
                employee.Department = department;

            Console.Write($"Email ({employee.Email}): ");
            string? email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                employee.Email = email;

            _context.SaveChanges();

            Console.WriteLine("Employee updated!");
            Console.ReadKey();
        }

        // DELETE
        public void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE EMPLOYEE ===");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var employee = _context.Employees
                .Include(e => e.AssignedAssets)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            // Remove asset assignments
            foreach (var asset in employee.AssignedAssets)
            {
                asset.EmployeeId = null;
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            Console.WriteLine("Employee deleted!");
            Console.ReadKey();
        }

        // ASSIGN ASSET TO EMPLOYEE
        public void AssignAssetToEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN ASSET TO EMPLOYEE ===");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var employee = _context.Employees
                .Include(e => e.AssignedAssets)
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == assetId);
            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            // ⭐ FIX: Update FK relation
            asset.EmployeeId = employee.Id;

            employee.AssignedAssets.Add(asset);
            _context.SaveChanges();

            Console.WriteLine($"Asset '{asset.Brand} {asset.ModelName}' assigned to {employee.FullName}.");
            Console.ReadKey();
        }

        // SHOW EMPLOYEE ASSETS
        public void ShowEmployeeAssets()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE ASSETS ===");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var employee = _context.Employees
                .Include(e => e.AssignedAssets)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nEmployee: {employee.FullName}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine("\nAssigned Assets:");

            if (!employee.AssignedAssets.Any())
            {
                Console.WriteLine("No assets assigned.");
            }
            else
            {
                foreach (var asset in employee.AssignedAssets)
                {
                    Console.WriteLine($"{asset.Id} | {asset.Brand} | {asset.ModelName} | {asset.AssetType}");
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadKey();
        }
    }
}