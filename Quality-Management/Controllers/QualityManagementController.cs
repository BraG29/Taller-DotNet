using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quality_Management.DataAccess;
using Quality_Management.DTO;
using Quality_Management.Model;
using Quality_Management.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quality_Management.Controllers
{

    [ApiController]
    [Route("quality-management-api")]
    public class QualityManagementController : Controller
    {
        private readonly IProcedureService _procedureService;
        private readonly IRealTimeMetricsService _realTimeMetricsService;
        private readonly IOfficeRepository _officeRepository;

        public QualityManagementController(IProcedureService procedureService, 
            IRealTimeMetricsService realTimeMetricsService, IOfficeRepository officeRepository)
        {
            _procedureService = procedureService;
            _realTimeMetricsService = realTimeMetricsService;
            _officeRepository = officeRepository;
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
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
            }

        }


        [HttpPut]
        [Route("finishProcedure/{Id}")]
        public async Task<ActionResult> FinishProcedure(long id, [FromBody] DateTime procedureEnd)
        {
            try
            {
                await _procedureService.EndProcedure(id, procedureEnd);
                
                await _realTimeMetricsService.SendMetric(_realTimeMetricsService.PositionReleased,
                    _officeRepository.FindByProcedure(id).OfficeId);
                
                return Ok("Tramite finalizado con exito. ");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al finalizar: " + ex);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
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

        //Esto esta funcionando para pedir promedio de duracion de tramites por oficina
        [HttpGet]
        [Route("getProceduresAverageTime/{officeId}")]
        public async Task<ActionResult<string>> getProceduresAverageTime(string officeId)
        {
            try
            {
                var average = await _procedureService.ProceduresAverageTime(officeId);
                return Ok(average);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound("Fallo al obtener: " + ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("getProceduresAmount/{officeId}")]
        public async Task<ActionResult<long>> getProceduresAmount(string officeId)
        {
            try
            {
                var amount = await _procedureService.ProceduresAmount(officeId);
                return Ok(amount);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest("Fallo al obtener: " + ex);
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

        [HttpGet]
        [Route("getProceduresAverageWaitTime/{officeId}")]
        public async Task<ActionResult<string>> getProceduresAverageWaitTime(string officeId)
        {
            try
            {
                var average = await _procedureService.ProceduresAverageWaitTime(officeId);
                return Ok(average);
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
