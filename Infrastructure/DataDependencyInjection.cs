using Ardalis.GuardClauses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DataDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TaxDBConnectionString");

        Guard.Against.Null(connectionString, message: "Connection string 'TaxDBConnectionString' not found.");

        services.AddDbContext<ApplicationDBContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ApplicationDBContextInitialiser>();

        return services;
    }
}
