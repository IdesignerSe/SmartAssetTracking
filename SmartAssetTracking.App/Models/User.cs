namespace SmartAssetTracking.App.Models
{
    public class User
    {
        public int Id { get; set; }   // REQUIRED for EF Core + seeding

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}