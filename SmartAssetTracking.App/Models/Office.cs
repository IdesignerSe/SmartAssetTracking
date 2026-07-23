namespace SmartAssetTracking.App.Models
{
    public class Office
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }

        public List<Asset> Assets { get; set; } = new();
    }
}