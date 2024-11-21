using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using API_Gateway.DTOS;
using API_Gateway_Client.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static API_Gateway.Client.Pages.ClientMonitor;


namespace API_Gateway.Services;

public class CommercialOfficeService(HttpClient httpClient)
{
    public async Task<bool> CallCreateOffice(OfficeDTO office)
    {
        var positions = new List<object>();

        for (int i = 1; i <= office.PositionsAmount; i++)
        {
            positions.Add(new
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

    public async Task<IList<ClientOfficeDTO>> CallGetAllOffice()
    {
        //there supposedly shouldn't be a problem with the endpoint names of the Controller that calls this function
        var response = await httpClient.GetFromJsonAsync<IList<ClientOfficeDTO>>($"office/getAllOffices");

        return response;
    }


    public async Task<HttpResponseMessage> CallRegisterUser(string userCi, string officeId) {

        Console.WriteLine("I am CommercialOfficeService.CallRegisterUser and I got String Content: " + userCi + " / " + officeId);

        var url = $"office/registerUser?userId={userCi}&officeId={officeId}";

        return await httpClient.PutAsync(url, null);
    }

    public async Task<HttpResponseMessage> CallReleasePosition(string officeId, long placeNumber){

        Console.WriteLine("I am CommercialOfficeService.CallReleasePosition and I got String Content: " + officeId + " / " + placeNumber);

        var url = $"office/releasePosition?officeId={officeId}&placeNumber={placeNumber}";

        return await httpClient.PutAsync(url, null);
    }

    public async Task<HttpResponseMessage> CallNextUser(string officeId, long placeNumber)
    {
        Console.WriteLine("I am CommercialOfficeService.CallNextUser and I got String Content: " + officeId + " / " + placeNumber);

        var url = $"office/nextUser?officeId={officeId}&placeNumber={placeNumber}";
        return await httpClient.PutAsync(url, null);
    }

}