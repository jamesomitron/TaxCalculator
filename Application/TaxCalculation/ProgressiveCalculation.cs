using Domain.Entities;


namespace Application.TaxCalculation;

public class ProgressiveCalculation : ITaxCalculationByType
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

        if (TaxProgressiveRate.Count().Equals(0))
        {
            throw new ArgumentException("No Progressive Matrix Found");
        }

        if (!ValidateTaxMatrix(TaxProgressiveRate))
        {
            throw new ArgumentException("Invalid Progressive Matrix Found");
        }

        try
        {
            var filteredList = TaxProgressiveRate.Where(o => AnnualIncome > o.ToValue).ToList();
            var sortedList = filteredList.OrderBy(o => o.FromValue).ToList();

            decimal taxedIncome = 0 ;
            decimal totalTaxedIncome = 0;

            foreach (var matrix in sortedList)
            {
                var taxableValue = matrix.ToValue - taxedIncome;
                taxValue = taxValue + ((taxableValue * matrix.Rate) / 100);

                taxedIncome = matrix.ToValue;
                totalTaxedIncome = totalTaxedIncome + taxableValue;
            }

            filteredList = TaxProgressiveRate.Where(o => AnnualIncome >= o.FromValue && AnnualIncome <= o.ToValue).ToList();

            foreach(var matrix in filteredList)
            {
                var taxableValue = AnnualIncome - totalTaxedIncome;
                taxValue = taxValue + ((taxableValue * matrix.Rate) / 100);
            }
        }
        catch (ArithmeticException)
        {
            throw;
        }

        return taxValue;
    }

    private bool ValidateTaxMatrix(IEnumerable<TaxProgressiveRates> TaxProgressiveMatrix)
    {
        int currentFrom = 0;
        int currentTo = 0;

        var sortedList = TaxProgressiveMatrix.OrderBy(o => o.FromValue).ToList();
        bool initialValue = true;

        foreach (var matrix in sortedList)
        {

            if (!initialValue)
            {
                if((currentTo + 1) != matrix.FromValue)
                {
                    return false;
                }

                if (matrix.FromValue > matrix.ToValue)
                {
                    return false;
                }

                
            }

            if (initialValue)
            {
                //First 0 Confirmation
                initialValue = false;

                if (matrix.FromValue != 0)
                {
                    return false;
                }

                if (matrix.FromValue > matrix.ToValue)
                {
                    return false;
                }
            }

            currentFrom = matrix.FromValue;
            currentTo = matrix.ToValue;
        }

        return true;
    }
}
