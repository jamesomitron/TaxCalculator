using Domain.Entities;

namespace Domain.Repository;

public interface ITaxConfigValueRepository
{
    Task<IEnumerable<TaxConfigValue>> GetConfigValueAsync();
}
