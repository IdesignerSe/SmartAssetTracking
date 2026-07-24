namespace SmartAssetTracking.App.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }

        // Relation: Employee → Assets
        public List<Asset>? AssignedAssets { get; set; }
    }
}