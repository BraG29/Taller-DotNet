using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Services;

public class CommercialOfficeService(HttpClient httpClient)
{
    public string Hi()
    {
        Console.WriteLine("Llamando enpoint en commercial office");
        var response = httpClient
            .PutAsJsonAsync("office/releasePosition?officeId=OFI-1&placeNumber=1", new MultipartContent());
        return response.Result.Content.ReadAsStringAsync().Result;
        // return "Hola Mundo";
    }
    
}