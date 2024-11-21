using API_Gateway.DTOS;
using API_Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers;

[ApiController]
[Route("/api-gateway")]
public class ApiGatewayController : Controller
{
    private readonly CommercialOfficeService _commercialOfficeService;
    private readonly QualityManagementService _qualityManagementService;
    private readonly AuthenticationService _authenticationService;

    public ApiGatewayController(CommercialOfficeService commercialOfficeService, QualityManagementService qualityManagementService, AuthenticationService authenticationService)
    {
        _commercialOfficeService = commercialOfficeService;
        _qualityManagementService = qualityManagementService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        return Ok(await _authenticationService.CallLogin(request));
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
}