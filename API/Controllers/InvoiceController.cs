using AssetManagement.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AssetManagement.Models;
using System.Collections.Generic;
using AssetManagement.Dto;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using AssetManagement.BusinessLayer;

namespace AssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly AssetsDbContext _assetsDbContext;

        public InvoiceController(AssetsDbContext assetsDbContext)
        {
            _assetsDbContext = assetsDbContext;
        }

        [HttpPost]
        public async Task<IEnumerable<InvoiceDto>> GenerateInvoice(GenerateInvoiceParamDto param)
        {
            return BL_Invoice.GenerateInvoice(param);
        }

        [HttpPost("get-by-issued-date")]
        public async Task<IEnumerable<AssetDto>> GetInvoiceItemsByIssuedDate(IssuedDateDto param)
        {
            return BL_Invoice.GetInvoiceItemsByIssuedDate(param.date);
        }
    }
}
