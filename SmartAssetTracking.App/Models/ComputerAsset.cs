namespace SmartAssetTracking.App.Models
{
    public class ComputerAsset : Asset
    {
        public string? CPU { get; set; }
        public string? RAM { get; set; }
        public string? Storage { get; set; }
        public string? GPU { get; set; }
    }
}