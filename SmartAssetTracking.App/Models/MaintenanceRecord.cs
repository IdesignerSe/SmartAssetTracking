namespace SmartAssetTracking.App.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }

        // Relation: Maintenance → Asset
        public int AssetId { get; set; }
        public Asset? Asset { get; set; }
    }
}