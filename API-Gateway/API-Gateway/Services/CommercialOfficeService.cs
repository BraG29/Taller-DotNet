using System.Runtime.InteropServices.JavaScript;
using API_Gateway.DTOS;

namespace API_Gateway.Services;

public class CommercialOfficeService(HttpClient httpClient)
{
    public async Task<bool> CallCreateOffice(OfficeDTO office)
    {
        var positions = new List<object>();

        for (int i = 1; i <= office.PositionsAmount; i++)
        {
            positions.Add( new
            {
                Number = i,
                Available = false
            });
        }

        var officeRequest = new
        {
            Identificator = office.Id,
            AttentionPlaces = positions
        };

        var response = await httpClient.PostAsJsonAsync("office/createOffice", officeRequest);

        return response.IsSuccessStatusCode;
    }

    public async Task CallDeleteOffice(string officeId)
    {
        await httpClient.DeleteAsync($"office/deleteOffice/{officeId}");
    }
    
}