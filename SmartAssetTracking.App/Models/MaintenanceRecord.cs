namespace SmartAssetTracking.App.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        public DateTime LastMaintenance { get; set; }
        public DateTime NextMaintenance { get; set; }
        public string Notes { get; set; }
    }
}