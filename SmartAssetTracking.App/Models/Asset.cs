namespace SmartAssetTracking.App.Models
{
    public class Asset
    {
        public int Id { get; set; }

        // Basic info
        public string AssetType { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string SerialNumber { get; set; }

        // Purchase info
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePriceUSD { get; set; }
        public decimal LocalPrice { get; set; }
        public DateTime WarrantyExpiration { get; set; }

        // Office relation (LEVEL 3)
        public int OfficeId { get; set; }
        public Office Office { get; set; }

        // Employee relation (LEVEL 5)
        public int? EmployeeId { get; set; }
        public Employee AssignedEmployee { get; set; }
    }
}