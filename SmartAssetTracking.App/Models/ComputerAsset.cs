namespace SmartAssetTracking.App.Models
{
    public abstract class ComputerAsset : Asset
    {
        // Extra technical specifications (optional but useful)
        public string? CPU { get; set; }
        public string? RAM { get; set; }
        public string? Storage { get; set; }
        public string? GPU { get; set; }

        public ComputerAsset()
        {
            AssetType = "Computer";
        }
    }
}