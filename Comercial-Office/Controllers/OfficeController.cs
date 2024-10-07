using Comercial_Office.DTO;
using Comercial_Office.Model;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;


namespace Comercial_Office.Controllers
{
    [ApiController]
    [Route("office")]
    public class OfficeController : Controller
    {

        private readonly IOfficeRepository _officeRepository;

        public OfficeController(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        /*
        public IActionResult Index()
        {
            return Ok();
        }*/




        [HttpGet]
        [Route("getAllOffices")]
        public ActionResult<IList<OfficeDTO>> getOffices()
        {

            //obtengo mi lista de oficinas en memoria.
            IList<Office> offices = this._officeRepository.GetAll();


            //Armado de la lista de officinas DTO.
            if (offices != null)
            {

                IList<OfficeDTO> officesDTO = new List<OfficeDTO>();
                IList<AttentionPlaceDTO> attentionPlacesDTO = new List<AttentionPlaceDTO>();

                foreach (Office office in offices)
                {
                    if (office != null)
                    {
                        IList<AttentionPlace> attentionPlaces = office.AttentionPlaceList;

                        if(attentionPlaces != null)
                        {
                            foreach (AttentionPlace attentionPlace in attentionPlaces)
                            {
                                attentionPlacesDTO.Add(new AttentionPlaceDTO(attentionPlace.Number));
                            }
                        }

                        OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlacesDTO);
                        officesDTO.Add(officeDTO);
                    }

                }

                return Ok(officesDTO);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("getOffice/{id}")]
        public ActionResult<OfficeDTO> getOffice(string id) {
            //TESTEADO
            if (id != null)
            {
                Office office = this._officeRepository.GetOffice(id);

                //pasaje de objeto de dominio a DTO
                if (office != null)
                {
                    IList<AttentionPlace> attentionPlaceList = office.AttentionPlaceList;

                    IList<AttentionPlaceDTO> attentionPlaceListDTO = new List<AttentionPlaceDTO>();

                    if (attentionPlaceList != null)
                    {
                        foreach (AttentionPlace place in attentionPlaceList)
                        {
                            attentionPlaceListDTO.Add(new AttentionPlaceDTO(place.Number));
                        }
                    }

                    OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlaceListDTO);
                    return Ok(officeDTO);
                }
            }
            
            return NotFound();

        }


        [HttpDelete]
        [Route("deleteOffice/{id}")]
        public ActionResult<OfficeDTO> deleteOffice(string id)
        {
          
            if (id != null)
            {
                Office office = this._officeRepository.GetOffice(id);

                if (office != null)
                {
                    this._officeRepository.Delete(id); 

                    return Ok("Oficina eliminada");
                }
            }

            return NotFound();

        }

        [HttpPost]
        [Route("createOffice")]
        public ActionResult<OfficeDTO> createOffice(OfficeDTO officeDTO)
        {
          

            if (officeDTO != null)
            {
                //control de oficina existente
                if (officeDTO.Identificator != null)
                {
                    Office office = this._officeRepository.GetOffice(officeDTO.Identificator);

                    if (office != null)
                    {
                        return Conflict(new { message = "La oficina ya existe." });
                    }
                }


                //Pasaje de DTO de lugares de atencion a objetos de dominio.
                IList<AttentionPlace> attentionPlaces = new List<AttentionPlace>();

                if(officeDTO.Posts != null)
                {
                    IList<AttentionPlaceDTO> placesDTO = officeDTO.Posts;

                    foreach (AttentionPlaceDTO place in placesDTO)
                    {
                        attentionPlaces.Add(new AttentionPlace(place.Number, false));
                    }
                    
                }

                //Aca deberia crear  la queue y asignarla al objeto.

                Office newOffice = new Office(officeDTO.Identificator, null, attentionPlaces);

                this._officeRepository.Add(newOffice);
                
                return Created();
            }
            else
            {
                return StatusCode(500);
            }

        }

    }
}
