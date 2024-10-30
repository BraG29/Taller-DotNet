using Microsoft.EntityFrameworkCore;
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

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
        
        public IList<Procedure> FindAll()
        {
            return _context.Procedures.ToList();
            throw new NotImplementedException();
        }

        public Procedure FindById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Update(Procedure procedure)

        {
            _context.Entry(procedure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return "SI";
            }
            catch (Exception e)
            {
                return "NO" + e.ToString();
            }

        }
    }
}
