using API_Gateway.DTOS;

namespace API_Gateway.Services;

public class QualityManagementService(HttpClient httpClient)
{
    public async Task<bool> CallCreateOffice(OfficeDTO office)
    {
        var response = await httpClient.PostAsJsonAsync("/quality-management-api/create-office", office);

        return response.IsSuccessStatusCode;
    }

    public async Task CallDeleteOffice(string officeId)
    {
        await httpClient.DeleteAsync($"quality-management-api/delete-office/{officeId}");
    }
}