using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class AssetPrice
    {
        public int AssetPriceId { get; set; }
        public decimal Price { get; set; }   
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public int AssetId { get; set; }

        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }

    }
}
