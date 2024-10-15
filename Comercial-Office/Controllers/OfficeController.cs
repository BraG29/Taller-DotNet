using Comercial_Office.DTO;
using Comercial_Office.Model;
using Comercial_Office.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.InteropServices;


namespace Comercial_Office.Controllers
{
    [ApiController]
    [Route("office")]
    public class OfficeController : Controller
    {

        private readonly IOfficeService _officeService;


        public OfficeController( IOfficeService officeService)
        {
            _officeService = officeService;
        }

        /// <summary>
        /// Obtiene todas las oficinas del sistema
        /// </summary>
        /// <returns> Devuelve una lista de oficinas </returns>
        /// <response code="200"> Retorna una lista de todas las oficinas</response>
        /// <response code="505"> Si no puedo retornar la lista </response>
        [HttpGet]
        [Route("getAllOffices")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IList<OfficeDTO>> getOffices()
        {
            try
            {
                IList<OfficeDTO> offices = this._officeService.GetAll();
                return Ok(offices);
                
            } catch (Exception ex) {

                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);

            }
        }


        /// <summary>
        /// Buscar una oficina por ID
        /// </summary>
        /// <param name="id"> Utiliza el identificador para buscar la oficina</param>
        /// <returns>Devuelve una oficina </returns>
        /// <response code="200"> Si encontró una oficina </response>
        /// <response code="404"> Si no se pudo encontrar la oficina </response>
        [HttpGet]
        [Route("getOffice/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OfficeDTO> getOffice(string id) {
            try
            {
                var officeDTO = this._officeService.GetOffice(id);
                return Ok(officeDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Falló al encontrar: " + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Identificador invalido" + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }


        /// <summary>
        /// Eliminar Oficina
        /// </summary>
        /// <param name="id">  Utiliza el identificador para buscar y eliminar una oficina </param>
        /// <response code="200"> Retorna un mensaje si eliminó la oficina</response>
        /// <response code="404"> Si no se pudo encontrar la oficina a eliminar </response>
        [HttpDelete]
        [Route("deleteOffice/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OfficeDTO> deleteOffice(string id)
        {

            try
            {
                _officeService.DeleteOffice(id);
                return Ok("Oficina eliminada con exito");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al eliminar: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }



        /// <summary>
        /// Crear oficina
        /// </summary>
        /// <param name="officeDTO">Utiliza los parámetros del body para crear el objeto</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "identificator": "exampleOffice",
        ///         "attentionPlaces": [
        ///             {
        ///                 "number": 0,
        ///                 "available": false
        ///             },
        ///             {
        ///                 "number": 1,
        ///                 "available": false
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Si se pudo crear la oficina</response>
        /// <response code="400">Si no se pudo crear la oficina, por un error en los argumentos enviados</response>
        /// <response code="500">Si ocurrió un error interno</response>
        [HttpPost]
        [Route("createOffice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OfficeDTO> createOffice(OfficeDTO officeDTO)
        {
            try
            {
                _officeService.CreateOffice(officeDTO);
                return Ok("Oficina creada con exito.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Fallo al crear la oficina: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }


        public ActionResult<string> registerUser(string userId, string officeId)
        {

            try
            {
                _officeService.RegisterUser(userId, officeId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al eliminar: " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }

        }


        public ActionResult<string> releasePosition(string officeId, long placeNumber)
        {

            return Ok();
        }

        
    }

}
