using AssetManagement.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace AssetManagement.DataLayer
{
    public class DL_Invoice
    {
		public static DataTable GetInvoiceItemsByIssuedDate(string issuedDate)
		{
			var sql = string.Format(
                        @"DECLARE @IssuedDate VARCHAR(10) = '{0}'

						SELECT A.AssetId
							, A.Name
							, IIF (A.ValidFrom = '0001-01-01 00:00:00.0000000', NULL, A.ValidFrom) ValidFrom
							, IIF (A.ValidTo   = '9999-12-31 23:59:59.9999999', NULL, A.ValidTo)   ValidTo
							, AP.Price
							FROM Assets A
							CROSS APPLY (
								SELECT TOP 1 Price 
								FROM AssetPrice
								WHERE AssetId = A.AssetId
									AND @IssuedDate BETWEEN CAST(StartDate AS DATE) AND CAST(DATEADD(DAY, -1, EndDate) AS DATE)
									ORDER BY AssetPriceId DESC
							) AP
							WHERE (@IssuedDate BETWEEN CAST(ValidFrom AS DATE) AND CAST(ValidTo AS DATE))
								AND IsDeleted = 0
								AND (@IssuedDate BETWEEN CAST(CreatedDate AS DATE) AND CAST(DATEADD(DAY, -1, DeletedDate) AS DATE))  
								AND @IssuedDate <= CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+08:00') AS DATE)", issuedDate
						);

            DataTable dt = new DataTable();

			var conn = new SqlConnection(DL_Helpers.GetConnectionString());
            var cmd = new SqlCommand(sql, conn);
            var da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            return dt;
        }

        public static DataTable GenerateInvoice(GenerateInvoiceParamDto param)
        {
			DataTable dt = new DataTable();

            var sql = string.Format(
                        @"DECLARE @Month INT = {0}
						DECLARE @Year  INT = {1}
						DECLARE @BuildDate VARCHAR(10) = CONCAT(@Month, '/', '28', '/', @Year)
						DECLARE @NumberOfDays INT = datediff(day, @BuildDate, dateadd(month, 1, @BuildDate))

						DECLARE @Dates TABLE
						(
							Id INT,
							Date DateTime
						)

						DECLARE @Number INT = 1

						WHILE @Number <= @NumberOfDays
						BEGIN
						   INSERT INTO @Dates(Id, Date)
						   VALUES (@Number, CONCAT(@Month, '/', @Number, '/', @Year) )

						   SET @Number = @Number + 1
						END

						SELECT D.Date IssuedDate
							, P.Price TotalAmount
							, @Month  CycleMonth
							, @Year   CycleYear
							FROM @Dates D
							CROSS APPLY (
								SELECT SUM(AP.Price) Price
								FROM Assets A
								CROSS APPLY (
									SELECT TOP 1 Price 
									FROM AssetPrice
									WHERE AssetId = A.AssetId
										AND D.Date BETWEEN CAST(StartDate AS DATE) AND CAST(DATEADD(DAY, -1, EndDate) AS DATE)
									ORDER BY AssetPriceId DESC
								) AP
								WHERE (D.Date BETWEEN CAST(ValidFrom AS DATE) AND CAST(ValidTo AS DATE))
									--AND (D.Date BETWEEN CAST(CreatedDate AS DATE) AND CAST(DATEADD(DAY, -1, DeletedDate) AS DATE))  
									AND D.Date <= CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+08:00') AS DATE)	
									AND IsDeleted = 0
			
							) P
							WHERE Price IS NOT NULL

                        ", param.Month, param.Year);

            var conn = new SqlConnection(DL_Helpers.GetConnectionString());
            var cmd = new SqlCommand(sql, conn);
            var da = new SqlDataAdapter(cmd);

			try
			{
			   da.Fill(dt);
			}
			catch { }

           

            return dt;
        }
    }
}
