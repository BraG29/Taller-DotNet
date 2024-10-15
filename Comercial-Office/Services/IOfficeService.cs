using Comercial_Office.Model;
using Comercial_Office.DTO;

namespace Comercial_Office.Services
{
    public interface IOfficeService
    {
        public void CreateOffice(OfficeDTO office);
        public void UpdateOffice(OfficeDTO office);
        public void DeleteOffice(string id);
        public OfficeDTO GetOffice(string id);
        public IList<OfficeDTO> GetAll();

        public void RegisterUser(string  userId, string officeId);

        public void ReleasePosition(string officeId, string placeNumber);
    }
}
