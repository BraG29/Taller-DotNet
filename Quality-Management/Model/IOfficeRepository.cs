namespace Quality_Management.Model;

public interface IOfficeRepository
{
    public Task<IList<Office>> FindAll();
    public bool ExistsById(string officeId);
    public Office? FindById(string officeId);
}