using Comercial_Office.Model;
using Microsoft.AspNetCore.Mvc;


namespace Comercial_Office.Controllers
{
    [ApiController]
    [Route("office")]
    public class OfficeController : Controller
    {

        private readonly IOfficeRepository _officeRepository;


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("getAllOffices")]
        public IActionResult getOffices()
        {
            this._officeRepository.GetAll();
            return View();
        }
    }
}
