using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAssetTracking.App.Models
{
    public class MaintenanceRecord
    {
        [Key]
        public int Id { get; set; }

        // FK → Asset
        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        // Description of the maintenance
        public string Description { get; set; } = string.Empty;

        // Cost of the maintenance
        public decimal Cost { get; set; }

        // Date of maintenance
        public DateTime Date { get; set; }
    }
}