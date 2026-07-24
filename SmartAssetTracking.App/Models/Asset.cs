namespace SmartAssetTracking.App.Models
{
    public class Asset
    {
        public int Id { get; set; }

        // Basic info
        public string AssetType { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // Purchase info
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }

        // Office relation
        public int OfficeId { get; set; }
        public Office Office { get; set; } = null!;

        // Employee assignment (Level 5)
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Maintenance tracking (Level 5)
        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();
    }
}