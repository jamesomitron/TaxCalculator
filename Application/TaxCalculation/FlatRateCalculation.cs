using Domain.Entities;


namespace Application.TaxCalculation;

public class FlatRateCalculation : ITaxCalculationByType
{
    public decimal Execute(decimal AnnualIncome,
                            IEnumerable<TaxConfigValue> TaxConfigValues,
                            IEnumerable<TaxProgressiveRates> TaxProgressiveRate)
    {
        decimal taxValue = 0;
        decimal flatRate = 0;

        if (TaxConfigValues == null)
        {
            throw new ArgumentException("No Tax Configuration Value Found");
        }

        if (TaxConfigValues.Count() == 0)
        {
            throw new ArgumentException("No Tax Configuration Value Found");
        }

        if (AnnualIncome.Equals(0))
        {
            return taxValue;
        }

        if (AnnualIncome < 0)
        {
            throw new ArgumentException("Invalid Annual Income");
        }

        var checkFlatRate = TaxConfigValues.FirstOrDefault(o => o.Item == "FlatRate");  

        if (checkFlatRate == null)
        {
            throw new ArgumentException("No Flat Rate Found");
        }

        flatRate = checkFlatRate.Value;

        if (flatRate <= 0)
        {
            throw new ArgumentException("Invalid Flat Rate");
        }

        try
        {
            taxValue = (flatRate * AnnualIncome)/100;
        }
        catch(ArithmeticException)
        {
            throw; 
        }

        return taxValue;
    }
}
