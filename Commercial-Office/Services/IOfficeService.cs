using Commercial_Office.DTO;

namespace Commercial_Office.Services
{
    public interface IOfficeService
    {
        public void CreateOffice(OfficeDTO office);
        public void UpdateOffice(string officeId, IList<AttentionPlaceDTO> places);
        public void DeleteOffice(string id);
        public OfficeDTO GetOffice(string id);
        public IList<OfficeDTO> GetAll();

        public void RegisterUser(string  userId, string officeId);

        public void ReleasePosition(string officeId, long placeNumber);
    }
}
