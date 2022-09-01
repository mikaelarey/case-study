using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AssetManagement.DataLayer
{
    public class AssetsDbContext : DbContext
    {
        public AssetsDbContext(DbContextOptions<AssetsDbContext> options) : base(options)
        {
            CreateDataBaseIfNotExists();
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItems> InvoiceItems { get; set; }
        public DbSet<AssetPrice> AssetPrice { get; set; }

        protected override void OnModelCreating(ModelBuilder mb) { }

        private void CreateDataBaseIfNotExists()
        {
            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                if (dbCreator != null)
                {
                    if (!dbCreator.CanConnect())
                        dbCreator.Create();

                    if (!dbCreator.HasTables())
                        dbCreator.CreateTables();
                }
            }
            catch { }
        }
    }
}
