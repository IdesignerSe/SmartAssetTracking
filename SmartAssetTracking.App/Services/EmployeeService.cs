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

        // ============================================================
        // ADD EMPLOYEE
        // ============================================================
        public void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== ADD EMPLOYEE ===");

            Console.Write("Full Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Department: ");
            string dept = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            var emp = new Employee
            {
                FullName = name,
                Department = dept,
                Email = email
            };

            _context.Employees.Add(emp);
            _context.SaveChanges();

            Console.WriteLine("Employee added!");
            Console.ReadKey();
        }

        // ============================================================
        // SHOW EMPLOYEES
        // ============================================================
        public void ShowEmployees()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEES ===");

            var employees = _context.Employees
                .Include(e => e.Assets)
                .ToList();

            if (!employees.Any())
            {
                Console.WriteLine("No employees found.");
                Console.ReadKey();
                return;
            }

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.Id} | {e.FullName} | {e.Department} | Assets: {e.Assets.Count}");
            }

            Console.ReadKey();
        }

        // ============================================================
        // SHOW EMPLOYEE DETAILS
        // ============================================================
        public void ShowEmployeeDetails()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE DETAILS ===");

            Console.Write("Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var emp = _context.Employees
                .Include(e => e.Assets)
                .ThenInclude(a => a.MaintenanceRecords)
                .FirstOrDefault(e => e.Id == id);

            if (emp == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Name: {emp.FullName}");
            Console.WriteLine($"Department: {emp.Department}");
            Console.WriteLine($"Email: {emp.Email}");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine($"Assigned Assets: {emp.Assets.Count}");

            foreach (var a in emp.Assets)
            {
                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName}");
            }

            Console.WriteLine("----------------------------------------");
            Console.ReadKey();
        }

        // ============================================================
        // SHOW EMPLOYEE ASSETS (FIX FOR PROGRAM.CS)
        // ============================================================
        public void ShowEmployeeAssets()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE ASSETS ===");

            Console.Write("Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var emp = _context.Employees
                .Include(e => e.Assets)
                .ThenInclude(a => a.MaintenanceRecords)
                .FirstOrDefault(e => e.Id == id);

            if (emp == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Employee: {emp.FullName}");
            Console.WriteLine($"Department: {emp.Department}");
            Console.WriteLine($"Email: {emp.Email}");
            Console.WriteLine("----------------------------------------");

            if (!emp.Assets.Any())
            {
                Console.WriteLine("This employee has no assigned assets.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Assigned Assets:");
            foreach (var a in emp.Assets)
            {
                Console.WriteLine($"{a.Id} | {a.AssetType} | {a.Brand} {a.ModelName} | {a.SerialNumber}");
            }

            Console.WriteLine("----------------------------------------");
            Console.ReadKey();
        }

        // ============================================================
        // UPDATE EMPLOYEE
        // ============================================================
        public void UpdateEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE EMPLOYEE ===");

            Console.Write("Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Full Name ({emp.FullName}): ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) emp.FullName = name;

            Console.Write($"Department ({emp.Department}): ");
            string? dept = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dept)) emp.Department = dept;

            Console.Write($"Email ({emp.Email}): ");
            string? email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) emp.Email = email;

            _context.SaveChanges();

            Console.WriteLine("Employee updated!");
            Console.ReadKey();
        }

        // ============================================================
        // DELETE EMPLOYEE
        // ============================================================
        public void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE EMPLOYEE ===");

            Console.Write("Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var emp = _context.Employees
                .Include(e => e.Assets)
                .FirstOrDefault(e => e.Id == id);

            if (emp == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            if (emp.Assets.Any())
            {
                Console.WriteLine("Cannot delete employee: They still have assigned assets.");
                Console.WriteLine("Reassign or remove assets first.");
                Console.ReadKey();
                return;
            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();

            Console.WriteLine("Employee deleted!");
            Console.ReadKey();
        }

        // ============================================================
        // ASSIGN ASSET TO EMPLOYEE
        // ============================================================
        public void AssignAssetToEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN ASSET TO EMPLOYEE ===");

            Console.Write("Asset ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assetId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == assetId);
            var emp = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (asset == null)
            {
                Console.WriteLine("Asset not found.");
                Console.ReadKey();
                return;
            }

            if (emp == null)
            {
                Console.WriteLine("Employee not found.");
                Console.ReadKey();
                return;
            }

            asset.EmployeeId = employeeId;
            _context.SaveChanges();

            Console.WriteLine("Asset assigned to employee!");
            Console.ReadKey();
        }

        // ============================================================
        // MASS INSERT EMPLOYEES (FIX FOR PROGRAM.CS)
        // ============================================================
        public void Add10Employees()
        {
            Console.Clear();
            Console.WriteLine("=== MASS INSERT: 10 EMPLOYEES ===");

            for (int i = 1; i <= 10; i++)
            {
                var emp = new Employee
                {
                    FullName = $"Employee {i}",
                    Department = "IT",
                    Email = $"employee{i}@company.com"
                };

                _context.Employees.Add(emp);
            }

            _context.SaveChanges();

            Console.WriteLine("10 employees added!");
            Console.ReadKey();
        }

        // ============================================================
        // EMPLOYEE REPORT (OPTIONAL BUT USED IN PROGRAM.CS)
        // ============================================================
        public void EmployeeReport()
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE REPORT ===");

            var employees = _context.Employees
                .Include(e => e.Assets)
                .ToList();

            if (!employees.Any())
            {
                Console.WriteLine("No employees found.");
                Console.ReadKey();
                return;
            }

            foreach (var e in employees)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"{e.Id} | {e.FullName} | {e.Department}");
                Console.WriteLine($"Email: {e.Email}");
                Console.WriteLine($"Assets: {e.Assets.Count}");
            }

            Console.WriteLine("----------------------------------------");
            Console.ReadKey();
        }
    }
}