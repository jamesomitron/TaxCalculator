using Domain.Entities;
using System.Linq;


namespace Application.TaxCalculation;

public class FlatValueCalculation : ITaxCalculationByType
{
    public decimal Execute(decimal AnnualIncome,
                            IEnumerable<TaxConfigValue> TaxConfigValues,
                            IEnumerable<TaxProgressiveRates> TaxProgressiveRate)
    {
        decimal taxValue = 0;

        if (AnnualIncome.Equals(0))
        {
            return taxValue;
        }

        decimal flatValue = 0;
        decimal flatValueRate = 0;
        decimal minimumAnnualIncome = 0;

        if (TaxConfigValues == null)
        {
            throw new ArgumentException("No Tax Configuration Value Found");
        }

        if (TaxConfigValues.Count() == 0)
        {
            throw new ArgumentException("No Tax Configuration Value Found");
        }

        var checkFlatValue = TaxConfigValues.FirstOrDefault(o => o.Item == "FlatValue");
        var checkFlatValueRate = TaxConfigValues.FirstOrDefault(o => o.Item == "FlatValueRate");
        var checkminimumAnnualIncome = TaxConfigValues.FirstOrDefault(o => o.Item == "MinimumAnnualIncome");

        if (checkFlatValue == null)
        {
            throw new ArgumentException("No Flat Value Found");
        }

        if (checkFlatValueRate == null)
        {
            throw new ArgumentException("No Flat Value Rate Found");
        }

        if (checkminimumAnnualIncome == null)
        {
            throw new ArgumentException("No Minimum Annual Income Found");
        }

        flatValue = checkFlatValue.Value;
        flatValueRate = checkFlatValueRate.Value;
        minimumAnnualIncome = checkminimumAnnualIncome.Value;

        try
        {
            if (AnnualIncome >= minimumAnnualIncome)
            {
                return flatValue;
            }

            taxValue = (flatValueRate * AnnualIncome) / 100;
        }
        catch(ArithmeticException)
        {
            throw;
        }

        return taxValue;
    }
}
