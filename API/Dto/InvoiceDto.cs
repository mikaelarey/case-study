using System.ComponentModel.DataAnnotations;
using System;

namespace AssetManagement.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public DateTime IssuedDate { get; set; }

        public string IssuedDateAsString { get; set; }

        public int CycleMonth { get; set; }

        public int CycleYear { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
