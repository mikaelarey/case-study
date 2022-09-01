using AssetManagement.DataLayer;
using AssetManagement.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssetManagement.BusinessLayer
{
    public class BL_Assets
    {
        public static List<AssetDto> GetAssets()
        {
			DataTable data = DL_Assets.GetAssets();
			var assets = new List<AssetDto>();

			foreach (DataRow item in data.Rows)
			{
				var asset = new AssetDto
				{
					AssetId   = Convert.ToInt32(item["AssetId"]),
					Name      = Convert.ToString(item["Name"]),
					ValidFrom = item["ValidFrom"] == DBNull.Value ? null : Convert.ToDateTime(item["ValidFrom"]),
					ValidTo   = item["ValidTo"] == DBNull.Value ? null : Convert.ToDateTime(item["ValidTo"]),
					Price     = Convert.ToDecimal(item["Price"])
				};

				assets.Add(asset);
			}

			return assets;
        }
    }
}
