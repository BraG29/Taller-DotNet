using Quality_Management.DTO;

namespace Quality_Management.Model
{
    public interface IProcedureRepository
    {

        public Task<long> Save(Procedure procedure);
        public void Delete(long id);
        public Procedure FindById(long id);
        public IList<Procedure> FindAll();


    }
}
