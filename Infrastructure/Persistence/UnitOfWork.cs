using Domain.Repository;
using Infrastructure.Data;
using Infrastructure.Persistence.Interface;
using Infrastructure.Repository;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _context;

    public ITaxPostalCodeRepository TaxPostalCode { get; private set; }
    public ITaxProgressiveRatesRepository TaxProgressiveRates { get; private set; }
    public ITaxRecordRepository TaxRecord { get; private set; }
    public ITaxConfigValueRepository TaxConfigValue { get; private set; }

    public UnitOfWork(ApplicationDBContext context)
    {
        _context = context;

        TaxPostalCode = new TaxPostalCodeRepository(_context);
        TaxProgressiveRates = new TaxProgressiveRatesRepository(_context);
        TaxRecord = new TaxRecordRepository(_context);
        TaxConfigValue = new TaxConfigValueRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
