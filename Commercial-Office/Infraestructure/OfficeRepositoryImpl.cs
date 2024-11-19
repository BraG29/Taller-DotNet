using Commercial_Office.DataAccess;
using Commercial_Office.Model;
using Microsoft.EntityFrameworkCore;
using static Commercial_Office.Model.Office;
using System.Collections.Concurrent;

namespace Commercial_Office.Infraestructure
{
    public class OfficeRepositoryImpl : IOfficeRepository
    {

        private IDictionary<string, Office> _Offices;

        private CommercialOfficeDbContext _DbContext;

        public OfficeRepositoryImpl(CommercialOfficeDbContext context) { 
            
            _DbContext = context;

            _Offices = new Dictionary<string, Office>();
        }

        public async Task Add(Office office)
        {
            await _DbContext.Offices.AddAsync(office);
            await _DbContext.SaveChangesAsync();
        }

        public async Task Update(Office office)
        {
            _DbContext.Entry(office).State = EntityState.Modified;
            await _DbContext.SaveChangesAsync();
        }
        public async Task Delete(Office office)
        {
            _DbContext.Offices.Remove(office);
            await _DbContext.SaveChangesAsync();
        }
        public async Task<Office> GetOffice(string id)
        {
            var office = await _DbContext.Offices
             .Include(o => o.AttentionPlaceList)
             .FirstOrDefaultAsync(o => o.Identificator == id);

            return office;
        }

        public async Task<IList<Office>> GetAll()
        {
            return await _DbContext.Offices
            .Include(o => o.AttentionPlaceList)
            .ToListAsync();
        }

        public async Task<bool> ExistById(string officeId)
        {
            return _DbContext.Offices != null && await _DbContext.Offices
                .AnyAsync(office => office.Identificator == officeId);
        }

    }
}
