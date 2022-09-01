using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Dto
{
    public class GenerateInvoiceParamDto
    {
        public int Month { get; set; }

        public int Year { get; set; }
    }
}
