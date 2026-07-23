namespace SmartAssetTracking.App.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePriceUSD { get; set; }
        public decimal LocalPrice { get; set; }
        public string SerialNumber { get; set; }
        public int? EmployeeId { get; set; }
        public Employee AssignedEmployee { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public int OfficeId { get; set; }
        public Office Office { get; set; }
    }
}