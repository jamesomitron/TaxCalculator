using Domain.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using taxRecordEntity = Domain.Entities.TaxRecord;

namespace Infrastructure.Repository;

public class TaxRecordRepository : ITaxRecordRepository
{
    private ApplicationDBContext _context;

    public TaxRecordRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public void Add(taxRecordEntity taxRecord)
    {
        _context.Set<taxRecordEntity>().Add(taxRecord);
    }

    public async Task<IEnumerable<taxRecordEntity>> GetTaxRecordAsync()
    {
        return await _context.Set<taxRecordEntity>().OrderByDescending(o => o.DateCreated).Take(5).ToListAsync();
    }

    public async Task<taxRecordEntity> GetTaxRecordAsync(Guid Id)
    {
        return await _context.Set<taxRecordEntity>().Where(o => o.Id.Equals(Id)).FirstOrDefaultAsync();
    }
}
