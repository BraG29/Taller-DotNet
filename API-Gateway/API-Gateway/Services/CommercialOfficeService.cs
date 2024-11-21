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

        //Console.WriteLine("I am CommercialOfficeService and I got String Content: " + data.ReadAsStringAsync());
        Console.WriteLine("I am CommercialOfficeService and I got String Content: " + userCi + " / " + officeId);

       
        Model model = new Model();
        model.UserCi = userCi;
        model.OfficeId = officeId;

        //var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        //HttpContent data = new StringContent(model);
        var url = $"office/registerUser?userId={userCi}&officeId={officeId}";

        return await httpClient.PutAsync(url, null);
        //return await httpClient.PutAsync($"office/registerUser", content );
        
    }

    public class Model
    {
        public string UserCi { get; set; }
        public string OfficeId { get; set; }
    }

}