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
        
        /// <summary>
        /// Funcion que crea un tramite
        /// </summary>
        /// <param name="procedure">Recibe un DTO con datos necesario para crear el tramite</param>
        /// <returns>Devuelve el identificador del mismo. </returns>
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

        /// <summary>
        /// Funcion encarga de finalizar un tramite, asignandole la fecha de finalizacion del mismo.
        /// </summary>
        /// <param name="id"> Identificador del tramite a finalizar </param>
        /// <param name="procedureEnd"> Fecha en la que finalizo el tramite</param>
        [HttpPut]
        [Route("finishProcedure/{Id}")]
        public async Task<ActionResult<List<ProcedureMetricsDTO>>> FinishProcedure(long id, [FromBody] DateTime procedureEnd)
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

        /// <summary>
        /// Endpoint para obtener valores que serán utilizados para construir graficas retroactivas
        /// </summary>
        /// <param name="officeId">Recibe el identificador de la oficina, de la que se obtienen los datos</param>
        /// <param name="range"> Recibe rango de tiempo, 0 = ultima semana, 1 = ultimo mes y 2 = ultimo año </param>
        /// <returns>Devuelve una lista de DTOs con los datos requeridos</returns>
        [HttpGet]
        [Route("getRetroactiveMetrics/{officeId}/{range}")]
        public async Task<ActionResult<List<ProcedureMetricsDTO>>> getRetroactiveMetrics(
            string officeId, 
            TimeRange range)
        {
            try
            {
                var average = await _procedureService.RetroactiveMetricsData(officeId, range);
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
