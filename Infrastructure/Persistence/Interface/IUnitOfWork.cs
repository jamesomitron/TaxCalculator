using Domain.Repository;

namespace Infrastructure.Persistence.Interface;

public interface IUnitOfWork : IDisposable
{
    ITaxPostalCodeRepository TaxPostalCode { get; }
    ITaxProgressiveRatesRepository TaxProgressiveRates { get; }
    ITaxRecordRepository TaxRecord { get; }
    ITaxConfigValueRepository TaxConfigValue { get; }
    int Complete();
    Task<int> CompleteAsync();
}
