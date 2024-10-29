using Commercial_Office.DTO;
using Commercial_Office.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;


namespace Commercial_Office.Controllers
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
        public ActionResult<IList<OfficeDTO>> GetOffices()
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
        public ActionResult<OfficeDTO> GetOffice(string id) {
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
        public ActionResult<OfficeDTO> DeleteOffice(string id)
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
        public ActionResult<OfficeDTO> CreateOffice(OfficeDTO officeDTO)
        {
            try
            {
                _officeService.CreateOffice(officeDTO);
                return Ok("Oficina creada con exito.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al liberar: " + ex.Message);
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


        /// <summary>
        /// Actualizar oficina
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina</param>
        /// <param name="places">Lista de puestos de la oficina</param>
        /// <remarks>
        /// Sample request:
        ///
        ///         "attentionPlaces": [
        ///             {
        ///                 "number": 5,
        ///                 "available": false
        ///             },
        ///             {
        ///                 "number": 6,
        ///                 "available": false
        ///             }
        ///         ]
        ///
        /// </remarks>
        /// <response code="200">Si se pudo actualizar la oficina</response>
        /// <response code="404">Si no existe la oficina</response>
        /// <response code="400">Si no se pudo actualizar la oficina, por argumentos invalidos</response>
        /// <response code="500">Si ocurrió un error interno</response>
        [HttpPut]
        [Route("updateOffice/{officeId}")]
        public ActionResult<IList<AttentionPlaceDTO>> UpdateOffice(string officeId, [FromBody] IList<AttentionPlaceDTO> places)
        {

            try
            {
                _officeService.UpdateOffice(officeId, places);
                return Ok("Puestos de oficina actualizados exitosamente.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al actualizar: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Fallo al actualizar: " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Fallo al actualizar: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Fallo al actualizar: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }
        }


        /// <summary>
        /// Registrar usuario
        /// </summary>
        /// <param name="userId"> Identificador del usuario</param>
        /// <param name="officeId"> Numero de la oficina a registrar el usuario</param>
        /// <response code="200"> Retorna un mensaje si registro el usuario</response>
        /// <response code="404"> Si no se pudo encontrar la oficina</response>
        /// <response code="500"> Si ocurrio un error interno</response>
        /// <response code="400"> Si se ingresaron parametros vacios</response>
        [HttpPut]
        [Route("registerUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string> RegisterUser(string userId, string officeId)
        {
            try
            {
                _officeService.RegisterUser(userId, officeId);
                return Ok("Usuario registrado con exito");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al registrar: " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Fallo al registrar: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }
        }

        /// <summary>
        /// Liberar puesto
        /// </summary>
        /// <param name="officeId"> Numero de la oficina donde se encuentra el puesto</param>
        /// <param name="placeNumber"> Numero de puesto, el mismo no puede ser menor a 0</param>
        /// <response code="200"> Retorna un mensaje si libero el puesto</response>
        /// <response code="404"> Si no se pudo encontrar la oficina</response>
        /// <response code="500"> Si ocurrio un error interno</response>
        /// <response code="400"> Si se ingresaron parametros vacios o incorrectos (Numero de puesto menor a 0)</response>
        [HttpPut]
        [Route("releasePosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ReleasePosition(string officeId, long placeNumber)
        {
            try
            {
                Console.WriteLine("Asignando usuario  al puesto :" + placeNumber + " de la oficina: " + officeId);
                await _officeService.ReleasePosition(officeId, placeNumber);
                return Ok("Oficina liberada con exito");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al liberar: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Fallo al liberar: " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Fallo al liberar: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Fallo al liberar: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }
           
        }

        /// <summary>
        /// Llamar al siguiente usuario en la cola
        /// </summary>
        /// <param name="officeId"> Numero de la oficina donde se encuentra el puesto</param>
        /// <param name="placeNumber"> Numero de puesto, el mismo no puede ser menor a 0</param>
        /// <response code="200"> Retorna un mensaje si asigno el usuario de la cola al puesto</response>
        /// <response code="404"> Si no se pudo encontrar la oficina</response>
        /// <response code="500"> Si ocurrio un error interno</response>
        /// <response code="400"> Si se ingresaron parametros vacios o incorrectos (Numero de puesto menor a 0)</response>
        [HttpPut]
        [Route("nextUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> NextUser(string officeId, long placeNumber)
        {
            try
            {
                await _officeService.CallNextUser(officeId, placeNumber);
                return Ok("Usuario asignado existosamente al puesto " + placeNumber);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Fallo al asignar: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Fallo al asignar: " + ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Fallo al asignar: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Fallo al asignar: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado." + ex.Message);
            }
        }
        
    }

}
