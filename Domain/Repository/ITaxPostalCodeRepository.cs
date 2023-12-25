using Domain.Entities;

namespace Domain.Repository;

public interface ITaxPostalCodeRepository
{
    Task<IEnumerable<TaxPostalCode>> GetPostalCodeAsync();
    Task<TaxPostalCode> GetPostalCodeByCodeAsync(string postalCode);
}
