namespace SmartAssetTracking.App.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }

        // Level 5 required fields
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Relation: Maintenance → Asset
        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;
    }
}