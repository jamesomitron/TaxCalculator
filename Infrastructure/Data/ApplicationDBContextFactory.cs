using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=.;Initial Catalog=TaxCalculationDB;User Id=sa;Password=pa$$1987W0rd;TrustServerCertificate=True";

            Guard.Against.Null(connectionString, message: "Connection string 'TaxDBConnectionString' not found.");

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDBContext>(); ;
            optionBuilder.UseSqlServer(connectionString);

            return new ApplicationDBContext(optionBuilder.Options);
        }
    }
}
