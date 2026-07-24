using SmartAssetTracking.App.Data;
using SmartAssetTracking.App.Models;

namespace SmartAssetTracking.App.Services
{
    public class LoginService
    {
        private readonly AssetDbContext _context;

        public LoginService(AssetDbContext context)
        {
            _context = context;
        }

        public User? Authenticate(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => 
                    u.Username.ToLower() == username.ToLower() &&
                    u.Password == password);
        }
    }
}