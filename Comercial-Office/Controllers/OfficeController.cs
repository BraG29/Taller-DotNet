using Comercial_Office.DTO;
using Comercial_Office.Model;
using Comercial_Office.Services;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
        [Route("getAllOffices")]
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



        [HttpGet]
        [Route("getOffice/{id}")]
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

       

        [HttpDelete]
        [Route("deleteOffice/{id}")]
        public ActionResult<OfficeDTO> deleteOffice(string id)
        {
            return Ok();
        } 
        
        

        [HttpPost]
        [Route("createOffice")]
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

        
    }
}
