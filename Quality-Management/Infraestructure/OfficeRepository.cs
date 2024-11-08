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

    public bool ExistsById(string officeId)
    {
        return _context.Offices.Any(office => office.OfficeId == officeId);
    }

    public Office FindById(string officeId)
    {
        return _context.Offices.Find(officeId);
    }

    public Office FindByProcedure(long procedureId)
    {
        Office? office = _context.Offices
            .FirstOrDefault(office => office.Procedures != null && office.Procedures
                .Any(procedure => procedure.Id == procedureId));

        if (office == null) throw new ArgumentException("No existe una oficina para ese tramite");

        return office;
    }
}