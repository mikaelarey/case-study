using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime IssuedDate { get; set; }

        [Required]
        public int CycleMonth { get; set; }

        [Required]
        public int CycleYear { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }
}
