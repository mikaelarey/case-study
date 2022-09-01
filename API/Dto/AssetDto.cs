using System.ComponentModel.DataAnnotations;
using System;

namespace AssetManagement.Dto
{
    public class AssetDto
    {
        public int AssetId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public decimal Price { get; set; }
    }
}
