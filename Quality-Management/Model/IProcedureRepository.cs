using Quality_Management.DTO;

namespace Quality_Management.Model
{
    public interface IProcedureRepository
    {

        public Task<long> Save(Procedure procedure);
        public Task Delete(Procedure procedure);
        public Task<Procedure> FindById(long id);
        public Task<IList<Procedure>> FindAll();
        public Task Update(Procedure procedure);

        public Task<IList<Procedure>> FindProceduresByOffice(string officeId);

    }
}
