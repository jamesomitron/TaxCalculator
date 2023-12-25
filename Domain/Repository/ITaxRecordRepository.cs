using Domain.Entities;

namespace Domain.Repository;

public interface ITaxRecordRepository
{
    Task<IEnumerable<TaxRecord>> GetTaxRecordAsync();

    void Add(TaxRecord taxRecord);
}
