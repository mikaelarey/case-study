using AssetManagement.Dto;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;

namespace AssetManagement.DataLayer
{
    public class DL_Assets
    {
        public static DataTable GetAssets()
        {
            DataTable data = new DataTable();

            var sql = @"SELECT A.AssetId
							, A.Name
							, IIF (A.ValidFrom = '0001-01-01 00:00:00.0000000', NULL, A.ValidFrom) ValidFrom
							, IIF (A.ValidTo   = '9999-12-31 23:59:59.9999999', NULL, A.ValidTo)   ValidTo
							, AP.Price
							FROM Assets A
							CROSS APPLY (
											SELECT TOP 1 Price 
												FROM AssetPrice
												WHERE AssetId = A.AssetId
												ORDER BY AssetPriceId DESC
										)	AP
							WHERE A.IsDeleted = 0";

            var conn = new SqlConnection(DL_Helpers.GetConnectionString());
            var cmd = new SqlCommand(sql, conn);
            var da = new SqlDataAdapter(cmd);

            da.Fill(data);

            return data;
        }
    }
}
