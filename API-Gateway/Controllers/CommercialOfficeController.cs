using API_Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers;

public class CommercialOfficeController(CommercialOfficeService officeService) : Controller
{

    [HttpGet]
    [Route("/hola")]
    public string Example()
    {
        return officeService.Hi();
    }
}