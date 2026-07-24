namespace SmartAssetTracking.App.Models
{
    public class MobileAsset : Asset
    {
        public string? OperatingSystem { get; set; }
        public string? ScreenSize { get; set; }
        public string? BatteryCapacity { get; set; }
    }
}