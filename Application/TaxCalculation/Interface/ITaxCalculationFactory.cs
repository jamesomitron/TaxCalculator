using Domain.Enums;

namespace Application.TaxCalculation;

public interface ITaxCalculationFactory
{
    ITaxCalculationByType GetTaxCalculationType(TaxType taxType);
}
