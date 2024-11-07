using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Quality_Management.DataAccess;
using Quality_Management.Model;

namespace Quality_Management.Infraestructure
{
    public class ProcedureRepositoryImpl : IProcedureRepository
    {
        private QualityManagementDbContext _context;

        public ProcedureRepositoryImpl(QualityManagementDbContext context)
        {
            _context = context;
        }

        public async Task<long> Save(Procedure procedure)
        {
             _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();     
            return procedure.Id;

        }

        public async Task Delete(Procedure procedure)
        {
            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IList<Procedure>> FindAll()
        {
            return _context.Procedures.ToList();
        }

        public async Task<Procedure> FindById(long id)
        {   
            return await _context.Procedures.FindAsync(id);
        }

        public async Task Update(Procedure procedure)

        {
            _context.Entry(procedure).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
