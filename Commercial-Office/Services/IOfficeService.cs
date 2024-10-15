using Commercial_Office.Model;
using Commercial_Office.DTO;

namespace Commercial_Office.Services
{
    public interface IOfficeService
    {
        public void CreateOffice(OfficeDTO office);
        public void UpdateOffice(OfficeDTO office);
        public void DeleteOffice(string id);
        public OfficeDTO GetOffice(string id);
        public IList<OfficeDTO> GetAll();

        public void RegisterUser(string  userId, string officeId);

        public void ReleasePosition(string officeId, long placeNumber);
    }
}
