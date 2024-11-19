using System.Collections.Concurrent;

namespace Commercial_Office.Model
{
    public interface IOfficeRepository
    {

        public Task Add(Office office);
        public Task Update(Office office);
        public Task Delete(Office office);
        public Task<Office> GetOffice(string id);
        public Task<IList<Office>> GetAll();
        public Task<bool> ExistById(string officeId);

    }
}
