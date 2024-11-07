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
        private readonly IProcedureService _procedureService;
        private readonly IRealTimeMetricsService _realTimeMetricsService;

        public QualityManagementController(IProcedureService procedureService, 
            IRealTimeMetricsService realTimeMetricsService)
        {
            _procedureService = procedureService;
            _realTimeMetricsService = realTimeMetricsService;
        }
        
        [HttpPost]
        [Route("startProcedure")]
        public async Task<ActionResult<long>> CreateProcedure(ProcedureDTO procedure)
        {
            try
            {
                await _realTimeMetricsService.SendMetric(_realTimeMetricsService.ClientLeavesTheQueue, 
                    procedure.OfficeId);
                long id = await _procedureService.CreateProcedure(procedure);
                return Ok(id);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al crear tramite: " + ex);
            }
            catch (DbUpdateException ex)
            {
                return Conflict("Fallo al crear el tramite: " + ex);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error al enviar metrica: {e.Message}");
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
            }

        }

        [HttpPut]
        [Route("finishProcedure/{Id}")]
        public async Task<ActionResult> FinishProcedure(long Id, [FromBody] DateTime ProcedureEnd)
        {
            try
            {
                //TODO: Se necesita la id de la oficina para enviar metricas en tiempo REAL
                await _procedureService.EndProcedure(Id, ProcedureEnd);
                return Ok("Tramite finalizado con exito. ");
            }
            catch (ArgumentNullException ex) {
                return BadRequest("Fallo al finalizar: " + ex);
            }
            catch (DbUpdateConcurrencyException ex)
            {
               return Conflict("Fallo al finalizar: " + ex);
            }
            catch (DbUpdateException ex)
            {
                return Conflict("Fallo al finalizar: " + ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("client-registration/{officeId}")]
        public async Task<ActionResult> ClientRegistration(string officeId)
        {
            try
            {
                await _realTimeMetricsService.SendMetric(_realTimeMetricsService.ClientEnterTheQueue, officeId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error al enviar metrica: {e.Message}");
                return NotFound(e.Message);
            }
        }
    }
}



/*
[HttpDelete]
[Route("deleteProcedure/{ProcedureId}")]
public async Task<ActionResult> deleteProcedure(long ProcedureId)
{

    try
    {
        await _procedureService.DeleteProcedure(ProcedureId);
        return Ok("Tramite Eliminado");
    }
    catch (ArgumentNullException ex)
    {
        return BadRequest("Fallo al eliminar: " + ex);
    }
    catch (DbUpdateConcurrencyException ex)
    {
        return Conflict("Fallo al eliminar: " + ex);
    }
    catch (DbUpdateException ex)
    {
        return Conflict("Fallo al eliminar: " + ex);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
    }

}

[HttpGet]
[Route("getProcedure/{ProcedureId}")]
public async Task<ActionResult<ProcedureDTO>> getProcedure(long ProcedureId)
{
    try
    {
        var procedure = await _procedureService.GetProcedure(ProcedureId);
        return Ok(procedure);
    }
    catch (ArgumentNullException ex)
    {
        return BadRequest("Fallo al obtener: " + ex);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
    }
}

[HttpGet]
[Route("getProcedures")]
public async Task<ActionResult<IList<ProcedureDTO>>> getProcedures()
{
    try
    {
        var procedures = await _procedureService.GetAll();
        return Ok(procedures);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
    }
}*/
