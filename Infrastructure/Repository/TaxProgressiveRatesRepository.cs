using Domain.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using taxProgressiveRatesEntity = Domain.Entities.TaxProgressiveRates;

namespace Infrastructure.Repository;

public class TaxProgressiveRatesRepository : ITaxProgressiveRatesRepository
{
    private ApplicationDBContext _context;

    public TaxProgressiveRatesRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<taxProgressiveRatesEntity>> GetProgressiveTaxMatrixAsync()
    {
        return await _context.Set<taxProgressiveRatesEntity>().ToListAsync();
    }
}
