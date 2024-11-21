using API_Gateway.DTOS;
using API_Gateway_Client.DTOs;

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

    public async Task<List<ProcedureMetricsDTO>> CallGetRetroactiveMetrics(string officeId, long interval){

        return await httpClient.GetFromJsonAsync<List<ProcedureMetricsDTO>>($"quality-management-api/getRetroactiveMetrics/"+officeId+"/"+interval);
    }
}