namespace SmartAssetTracking.App.Models
{
    public abstract class MobileAsset : Asset
    {
        // Mobile-specific specifications
        public string? OperatingSystem { get; set; }
        public string? ScreenSize { get; set; }
        public string? BatteryCapacity { get; set; }

        public MobileAsset()
        {
            AssetType = "Mobile";
        }
    }
}