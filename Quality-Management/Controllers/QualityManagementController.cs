using Microsoft.AspNetCore.Mvc;
using Quality_Management.Model;

namespace Quality_Management.Controllers
{

    [ApiController]
    [Route("quality-management")]
    public class QualityManagementController : Controller
    {

        private readonly IProcedureRepository _procedureRepository;

        public QualityManagementController(IProcedureRepository procedureRepository)
        {
            _procedureRepository = procedureRepository;
        }

        /*
        [HttpGet]
        [Route("getAllOffices")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IList<Procedure>> GetOffices()
        {
            try
            {
                //llamar repo
                return Ok();

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);

            }
        }*/


        [HttpPost]
        [Route("createProcedure")]
        public ActionResult<Procedure> CreateProcedure(Procedure procedure)
        {
            try
            {
                _procedureRepository.Add(procedure);
                return Ok("Tramite creado con exito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }

    }
}
