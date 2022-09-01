using AssetManagement.DataLayer;
using AssetManagement.Dto;
using AssetManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace AssetManagement.BusinessLayer
{
    public class BL_Invoice
    {
        public static List<InvoiceDto> GenerateInvoice(GenerateInvoiceParamDto param)
        {
            DataTable data = DL_Invoice.GenerateInvoice(param);
            var invoices = new List<InvoiceDto>();

            int Id = 1;

            foreach (DataRow item in data.Rows)
            {
                var invoice = new InvoiceDto
                {
                    Id = Id,
                    IssuedDate  = Convert.ToDateTime(item["IssuedDate"]),
                    TotalAmount = Convert.ToDecimal(item["TotalAmount"]),
                    CycleMonth  = Convert.ToInt32(item["CycleMonth"]),
                    CycleYear   = Convert.ToInt32(item["CycleYear"]),
                    IssuedDateAsString = Convert.ToDateTime(item["IssuedDate"]).ToString("MM/dd/yyyy")
                };

                invoices.Add(invoice);
                Id++;
            }

            return invoices;
        }

        public static List<AssetDto> GetInvoiceItemsByIssuedDate(string issuedDate)
        {
            DataTable data = DL_Invoice.GetInvoiceItemsByIssuedDate(issuedDate);
            var assets = new List<AssetDto>();

            foreach (DataRow item in data.Rows)
            {
                var asset = new AssetDto
                {
                    AssetId = Convert.ToInt32(item["AssetId"]),
                    Name = Convert.ToString(item["Name"]),
                    ValidFrom = item["ValidFrom"] == DBNull.Value ? null : Convert.ToDateTime(item["ValidFrom"]),
                    ValidTo = item["ValidTo"] == DBNull.Value ? null : Convert.ToDateTime(item["ValidTo"]),
                    Price = Convert.ToDecimal(item["Price"])
                };

                assets.Add(asset);
            }

            return assets;
        }
    }
}
