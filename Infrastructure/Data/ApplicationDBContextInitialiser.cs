using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationDBContextInitialiser
{
    private readonly ILogger<ApplicationDBContextInitialiser> _logger;
    private readonly ApplicationDBContext _context;

    public ApplicationDBContextInitialiser(ILogger<ApplicationDBContextInitialiser> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        // Seed Progressive Rate Matrix
        if (!_context.TaxProgressiveRates.Any())
        {
            var progressiveMatrix = new List<TaxProgressiveRates>()
            {
                new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 8350},
                new TaxProgressiveRates{ Rate = 15, FromValue = 8351, ToValue = 33950},
                new TaxProgressiveRates{ Rate = 25, FromValue = 33951, ToValue = 82250},
                new TaxProgressiveRates{ Rate = 28, FromValue = 82251, ToValue = 171550},
                new TaxProgressiveRates{ Rate = 33, FromValue = 171551, ToValue = 372950},
                new TaxProgressiveRates{ Rate = 35, FromValue = 372951},
            };

            await _context.TaxProgressiveRates.AddRangeAsync(progressiveMatrix);
            await _context.SaveChangesAsync();
        }

        // Seed Postal Code
        if (!_context.TaxPostalCode.Any())
        {
            var postalCodes = new List<TaxPostalCode>()
            {
                new TaxPostalCode{ Code = "7441", TaxCalculationType = TaxType.Progressive},
                new TaxPostalCode{ Code = "A100", TaxCalculationType = TaxType.FlatValue},
                new TaxPostalCode{ Code = "7000", TaxCalculationType = TaxType.FlatRate},
                new TaxPostalCode{ Code = "1000", TaxCalculationType = TaxType.Progressive}
            };

            await _context.TaxPostalCode.AddRangeAsync(postalCodes);
            await _context.SaveChangesAsync();
        }

        // Seed Tax Config Values
        if (!_context.TaxConfigValue.Any())
        {
            var configValues = new List<TaxConfigValue>()
            {
                new TaxConfigValue{ Item = "FlatRate", Value = (decimal)17.5},
                new TaxConfigValue{ Item = "FlatValue", Value = 10000},
                new TaxConfigValue{ Item = "FlatValueRate", Value = 5},
                new TaxConfigValue{ Item = "MinimumAnnualIncome", Value = 200000}
            };

            await _context.TaxConfigValue.AddRangeAsync(configValues);
            await _context.SaveChangesAsync();
        }
    }
}
