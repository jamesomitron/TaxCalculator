using Domain.Entities;

namespace Application.TaxCalculation;

public interface ITaxCalculationByType
{
    decimal Execute(decimal AnnualIncome, 
                    IEnumerable<TaxConfigValue> TaxConfigValues,
                    IEnumerable<TaxProgressiveRates> TaxProgressiveRate);
}
