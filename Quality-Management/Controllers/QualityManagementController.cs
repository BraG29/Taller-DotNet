using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quality_Management.DataAccess;
using Quality_Management.DTO;
using Quality_Management.Model;
using Quality_Management.Services;

namespace Quality_Management.Controllers
{

    [ApiController]
    [Route("quality-management-api")]
    public class QualityManagementController : Controller
    {
        private readonly IProcedureRepository _procedureRepository;
        private readonly IProcedureService _procedureService;
        

        public QualityManagementController(IProcedureRepository procedureRepository, IProcedureService procedureService)
        {
            _procedureRepository = procedureRepository;
            _procedureService = procedureService;
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
        public async Task<ActionResult<long>> CreateProcedure(ProcedureDTO procedure)
        {

            try
            {
                Console.WriteLine("Fecha: " + procedure.ProcedureStart.ToString());

                long id = await _procedureService.CreateProcedure(procedure);
                return Ok(id);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest("Fallo al crear tramite ");
            }
            catch(DbUpdateException ex)
            {
                return Conflict("Fallo al crear el tramite " + ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }

        [HttpGet]
        [Route("test")]
        public ActionResult<string> Test()
        {

            return "hola";

        }

    }
}
