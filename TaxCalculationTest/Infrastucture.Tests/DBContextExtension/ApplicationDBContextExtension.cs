
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TaxCalculationTest.Infrastucture.Tests.DBContextExtension;

public static class ApplicationDBContextExtension
{
    public static ApplicationDBContext AddTestData(this ApplicationDBContext context)
    {
        var postalCodes = new List<TaxPostalCode>()
        {
            new TaxPostalCode{ Code = "7441", TaxCalculationType = TaxType.Progressive},
            new TaxPostalCode{ Code = "A100", TaxCalculationType = TaxType.FlatValue},
            new TaxPostalCode{ Code = "7000", TaxCalculationType = TaxType.FlatRate},
            new TaxPostalCode{ Code = "1000", TaxCalculationType = TaxType.Progressive}
        };
        _ = context.SaveChanges();

        var progressiveMatrix = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 8350},
            new TaxProgressiveRates{ Rate = 15, FromValue = 8351, ToValue = 33950},
            new TaxProgressiveRates{ Rate = 25, FromValue = 33951, ToValue = 82250},
            new TaxProgressiveRates{ Rate = 28, FromValue = 82251, ToValue = 171550},
            new TaxProgressiveRates{ Rate = 33, FromValue = 171551, ToValue = 372950},
            new TaxProgressiveRates{ Rate = 35, FromValue = 372951, ToValue = int.MaxValue},
        };

        context.TaxProgressiveRates.AddRange(progressiveMatrix);

        

        context.TaxPostalCode.AddRange(postalCodes);

        var configValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{ Item = "FlatRate", Value = (decimal)17.5},
            new TaxConfigValue{ Item = "FlatValue", Value = 10000},
            new TaxConfigValue{ Item = "FlatValueRate", Value = 5},
            new TaxConfigValue{ Item = "MinimumAnnualIncome", Value = 200000}
        };

        context.TaxConfigValue.AddRange(configValues);

        var postalCode_7441 = context.TaxPostalCode.FirstOrDefault(o => o.Code.Equals("7441"));
        var postalCode_A100 = context.TaxPostalCode.Where(o => o.Code.Equals("A100")).FirstOrDefault();
        var postalCode_7000 = context.TaxPostalCode.Where(o => o.Code.Equals("7000")).FirstOrDefault();
        var postalCode_1000 = context.TaxPostalCode.Where(o => o.Code.Equals("1000")).FirstOrDefault();

        var taxRecords = new List<TaxRecord>()
        {
            new TaxRecord{ PostalCode = postalCode_7441, AnnualIncome = 500000, CalculatedTaxValue = 500},
            new TaxRecord{ PostalCode = postalCode_A100, AnnualIncome = 400000, CalculatedTaxValue = 400},
            new TaxRecord{ PostalCode = postalCode_7000, AnnualIncome = 300000, CalculatedTaxValue = 300},
            new TaxRecord{ PostalCode = postalCode_1000, AnnualIncome = 200000, CalculatedTaxValue = 200},
            new TaxRecord{ PostalCode = postalCode_7441, AnnualIncome = 10000, CalculatedTaxValue = 100},
            new TaxRecord{ PostalCode = postalCode_1000, AnnualIncome = 55000, CalculatedTaxValue = 5500},
            new TaxRecord{ PostalCode = postalCode_7441, AnnualIncome = 66000, CalculatedTaxValue = 6600},
            new TaxRecord{ PostalCode = postalCode_7000, AnnualIncome = 79000, CalculatedTaxValue = 7900},
            new TaxRecord{ PostalCode = postalCode_7441, AnnualIncome = 89000, CalculatedTaxValue = 890},
            new TaxRecord{ PostalCode = postalCode_A100, AnnualIncome = 23400, CalculatedTaxValue = 2340},
        };

        context.TaxRecord.AddRange(taxRecords);

        _ = context.SaveChanges();

        return context;
    }
}
