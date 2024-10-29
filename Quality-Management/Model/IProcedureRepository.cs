namespace Quality_Management.Model
{
    public interface IProcedureRepository
    {

        public Task<Procedure> Save(Procedure procedure);
        public void Delete(long id);
        public Procedure FindById(long id);
        public IList<Procedure> FindAll();

    }
}
