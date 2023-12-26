using Domain.Entities;

namespace Domain.Repository;

public interface ITaxRecordRepository
{
    Task<IEnumerable<TaxRecord>> GetTaxRecordAsync();

    Task<TaxRecord> GetTaxRecordAsync(Guid Id);

    void Add(TaxRecord taxRecord);
}
