using Domain.Enums;

namespace Application.TaxCalculation;

public class TaxCalculationFactory : ITaxCalculationFactory
{
    private Dictionary<TaxType, ITaxCalculationByType> _taxCalculationByType;

    public TaxCalculationFactory()
    {
        _taxCalculationByType = new Dictionary<TaxType, ITaxCalculationByType>
        {
            { TaxType.FlatRate, new FlatRateCalculation() },
            { TaxType.FlatValue, new FlatValueCalculation() },
            { TaxType.Progressive, new ProgressiveCalculation() }
        };
    }

    public ITaxCalculationByType GetTaxCalculationType(TaxType taxType)
    {
        return _taxCalculationByType[taxType];
    }
}
