using Domain.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using taxConfigValueEntity = Domain.Entities.TaxConfigValue;

namespace Infrastructure.Repository;

public class TaxConfigValueRepository : ITaxConfigValueRepository
{
    private ApplicationDBContext _context;

    public TaxConfigValueRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<taxConfigValueEntity>> GetConfigValueAsync()
    {
        return await _context.Set<taxConfigValueEntity>().ToListAsync();
    }
}
