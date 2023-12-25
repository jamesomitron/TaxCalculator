using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public DbSet<TaxRecord> TaxRecord => Set<TaxRecord>();
    public DbSet<TaxProgressiveRates> TaxProgressiveRates => Set<TaxProgressiveRates>();
    public DbSet<TaxPostalCode> TaxPostalCode => Set<TaxPostalCode>();
    public DbSet<TaxConfigValue> TaxConfigValue => Set<TaxConfigValue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
