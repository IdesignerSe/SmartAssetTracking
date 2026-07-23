namespace SmartAssetTracking.App.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public required string FullName { get; set; }
        public required string Department { get; set; }
        public required string Email { get; set; }

        public List<Asset> AssignedAssets { get; set; } = new();
    }
}