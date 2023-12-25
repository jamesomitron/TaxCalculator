using Domain.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using taxPostalCodeEntity = Domain.Entities.TaxPostalCode;

namespace Infrastructure.Repository;

public class TaxPostalCodeRepository : ITaxPostalCodeRepository
{
    private ApplicationDBContext _context;

    public TaxPostalCodeRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<taxPostalCodeEntity>> GetPostalCodeAsync()
    {
        return await _context.Set<taxPostalCodeEntity>().ToListAsync();
    }

    public async Task<taxPostalCodeEntity> GetPostalCodeByCodeAsync(string postalCode)
    {
        var code = await _context.Set<taxPostalCodeEntity>().Where(o => o.Code.Equals(postalCode)).FirstOrDefaultAsync();

        return code;
    }
}
