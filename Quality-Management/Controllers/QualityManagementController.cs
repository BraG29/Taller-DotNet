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

        [Route("finishProcedure/{id}")]
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


        [HttpPost]
        [Route("create-office")]
        public async Task<ActionResult<OfficeDTO>> CreateOffice([FromBody] OfficeDTO? office)
        {
            if (office == null) return BadRequest("La oficina no puede ser NULL");

            try
            {
                office = await _officeRepository.Save(office);
                return Ok(office);
                
            }
            catch(Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ha ocurrido un error a la hora de guardar los datos de la oficina");
            }
        }

        [HttpDelete]
        [Route("delete-office/{officeId}")]
        public async Task<ActionResult> DeleteOffice(string officeId)
        {
            try
            {
                await _officeRepository.Delete(_officeRepository.FindById(officeId));
                return Ok();
            }
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ha ocurrido un error a la hora de eliminar los datos de la oficina");
            }
            catch (ArgumentException e)
            {
                return BadRequest($"No existe una oficina con id: {officeId}");
            }
        }

    }
}
