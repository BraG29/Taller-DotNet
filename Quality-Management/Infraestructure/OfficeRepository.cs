using Microsoft.EntityFrameworkCore;
using Quality_Management.DataAccess;
using Quality_Management.Model;

namespace Quality_Management.Infraestructure;

public class OfficeRepository : IOfficeRepository
{
    private QualityManagementDbContext _context;

    public OfficeRepository(QualityManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Office>> FindAll()
    {
        return _context.Offices != null ? 
            await _context.Offices.ToListAsync() : 
            new List<Office>();
    }
}