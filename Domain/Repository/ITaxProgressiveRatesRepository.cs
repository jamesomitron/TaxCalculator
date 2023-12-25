using Domain.Entities;

namespace Domain.Repository;

public interface ITaxProgressiveRatesRepository
{
    Task<IEnumerable<TaxProgressiveRates>> GetProgressiveTaxMatrixAsync();
}
