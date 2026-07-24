using SmartAssetTracking.App.Models;

namespace SmartAssetTracking.App.Services
{
    public class LoginService
    {
        // ⭐ In-memory users (non-nullable strings)
        private readonly List<User> _users = new()
        {
            new User { Username = "admin",   Password = "1234",    Role = UserRole.Admin },
            new User { Username = "manager", Password = "manager", Role = UserRole.Manager },
            new User { Username = "employee",Password = "employee",Role = UserRole.Employee }
        };

        public User Login()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== LOGIN ===");

                Console.Write("Username: ");
                string username = Console.ReadLine() ?? string.Empty;

                Console.Write("Password: ");
                string password = Console.ReadLine() ?? string.Empty;

                var user = _users.FirstOrDefault(u =>
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == password);

                if (user == null)
                {
                    Console.WriteLine("Invalid login. Try again.");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine($"Login successful! Role: {user.Role}");
                Console.ReadKey();
                return user;
            }
        }
    }
}