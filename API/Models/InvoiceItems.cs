using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class InvoiceItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int InvoiceId { get; set; }


        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
    }
}
