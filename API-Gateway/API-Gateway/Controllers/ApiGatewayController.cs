using API_Gateway.DTOS;
using API_Gateway_Client.DTOs;
using API_Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Gateway.Controllers;

[ApiController]
[Route("/api-gateway")]
public class ApiGatewayController : Controller
{
    private readonly CommercialOfficeService _commercialOfficeService;
    private readonly QualityManagementService _qualityManagementService;
    private readonly AuthenticationService _authenticationService;

    public ApiGatewayController(CommercialOfficeService commercialOfficeService,
        QualityManagementService qualityManagementService,
        AuthenticationService authenticationService)
    {
        _commercialOfficeService = commercialOfficeService;
        _qualityManagementService = qualityManagementService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {

        try
        {
            var respose = await _authenticationService.CallLogin(request);
            return Ok(respose);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authenticationService.CallRegister(request);
            return Ok(response);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
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
    [Route("/registerClient/{userCi}/{officeId}")]
    public async Task<HttpResponseMessage> RegisterClient(string userCi, string officeId){

        //Console.WriteLine("I am Api-Gateway Controller and I got String Content: " +  data.ReadAsStringAsync());
        Console.WriteLine("I am Api-Gateway Controller and I got String Content: " + userCi +" / "+ officeId);
        return await _commercialOfficeService.CallRegisterUser(userCi,officeId);
    }

    [HttpPut]
    [Route("/releasePosition/{officeId}/{placeNumber}")]
    public async Task<HttpResponseMessage> ReleasePosition(string officeId, long placeNumber){

        return await _commercialOfficeService.CallReleasePosition(officeId, placeNumber);
    }

    [HttpPut]
    [Route("/nextUser/{officeId}/{placeNumber}")]
    public async Task<HttpResponseMessage> NextUser(string officeId, long placeNumber){

        return await _commercialOfficeService.CallNextUser(officeId, placeNumber);
    }
}