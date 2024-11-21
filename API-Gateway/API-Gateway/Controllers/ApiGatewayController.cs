using API_Gateway.DTOS;
using API_Gateway_Client.DTOs;
using API_Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers;

[ApiController]
[Route("/api-gateway")]
public class ApiGatewayController : Controller
{
    private readonly CommercialOfficeService _commercialOfficeService;
    private readonly QualityManagementService _qualityManagementService;

    public ApiGatewayController(CommercialOfficeService commercialOfficeService, QualityManagementService qualityManagementService)
    {
        _commercialOfficeService = commercialOfficeService;
        _qualityManagementService = qualityManagementService;
    }

    [HttpPost]
    [Route("/create-office")]
    public async Task<ActionResult> CreateOffice([FromBody] OfficeDTO office)
    {
        var callCommercialOffice = _commercialOfficeService.CallCreateOffice(office);
        var callQualityManagement = _qualityManagementService.CallCreateOffice(office);

        bool isSuccessCommercialOffice = await callCommercialOffice;
        bool isSuccessQualityManagement = await callQualityManagement;

        switch (isSuccessCommercialOffice)
        {
            case true when isSuccessQualityManagement:
                return Created();
            
            case true when !isSuccessQualityManagement:
                await _commercialOfficeService.CallDeleteOffice(office.Id);
                break;
            
            case false when isSuccessQualityManagement:
                await _qualityManagementService.CallDeleteOffice(office.Id);   
                break;
        }

        /*TODO: Devolver diferentes status segun el error especifico*/
        return Conflict("Ha habido un error al crear la oficina");
    }

    [HttpGet]
    [Route("/getAllOffices")]
    public async Task<IList<ClientOfficeDTO>> GetAllOffices(){
         
         return await _commercialOfficeService.CallGetAllOffice();
    }

    //🐎
    [HttpGet]
    [Route("/getRetroactiveMetrics/{officeId}/{interval}")]
    public async Task<IList<ProcedureMetricsDTO>> GetRetroactiveMetrics(string officeId, long interval){

        return await _qualityManagementService.CallGetRetroactiveMetrics(officeId, interval);
    }


    [HttpPut]
    [Route("/registerUser")]
    public async Task<HttpResponseMessage> RegisterUserCommercial(StringContent data){

        Console.WriteLine("I am Api-Gateway Controller and I got String Content: " +  data.ReadAsStringAsync());
        return await _commercialOfficeService.CallRegisterUser(data);
    }
}