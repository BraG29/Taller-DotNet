
namespace Comercial_Office.Model
{
    public interface IOfficeRepository
    {

        public void Add(Office office);
        public void Update(Office office);
        public void Delete(string id);
        public Office GetOffice(string id);
        public IList<Office> GetAll();

    }
}
