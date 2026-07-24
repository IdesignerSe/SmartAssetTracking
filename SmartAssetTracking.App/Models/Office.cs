namespace SmartAssetTracking.App.Models
{
    public class Office
    {
        public int Id { get; set; }

        public string OfficeName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        // Relation to assets
        public List<Asset> Assets { get; set; } = new();
    }
}