using System;

namespace AssetManagement.DataLayer
{
    public class DL_Helpers
    {
        public static string GetConnectionString()
        {
            var Host = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
            var Database = Environment.GetEnvironmentVariable("SQLSERVER_DB_NAME");
            var Username = "sa";
            var Password = Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");

            return $"Data Source={Host}; Initial Catalog={Database}; User ID={Username}; Password={Password}";
        }
    }
}
