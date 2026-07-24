namespace SmartAssetTracking.App.Models
{
    public class Asset
    {
        public int Id { get; set; }

        // Basic info
        public string AssetType { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // Purchase info
        public DateTime PurchaseDate { get; set; }

        // ORIGINAL FIELD (required for your existing services)
        public decimal PurchasePrice { get; set; }

        // Price in USD (required by PDF)
        public decimal PurchasePriceUSD { get; set; }

        // Converted local price (Level 3 requirement)
        public decimal LocalPrice { get; set; }

        // Required fields from PDF
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime WarrantyExpiration { get; set; }

        // Office relation (Level 3)
        public int OfficeId { get; set; }
        public Office Office { get; set; } = null!;

        // Employee assignment (Level 5)
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Maintenance tracking (Level 5)
        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();

        // Computed property for lifecycle (3 years lifetime)
        public string LifecycleStatus
        {
            get
            {
                var lifetime = PurchaseDate.AddYears(3);
                var remaining = lifetime - DateTime.Now;

                if (remaining.TotalDays < 90)
                    return "YELLOW";   // < 3 months
                if (remaining.TotalDays < 180)
                    return "RED";      // < 6 months
                return "NORMAL";
            }
        }
    }
}