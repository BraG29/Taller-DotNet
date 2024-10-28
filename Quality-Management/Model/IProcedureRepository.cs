namespace Quality_Management.Model
{
    public interface IProcedureRepository
    {

        public void Add(Procedure procedure);
        public void Update(Procedure procedure);
        public void Delete(string id);
        public Procedure GetOffice(string id);
        public IList<Procedure> GetAll();

    }
}
