using Comercial_Office.Model;
using Comercial_Office.DTO;

namespace Comercial_Office.Services 
{
    public class OfficeService : IOfficeService
    {

        private readonly IOfficeRepository _officeRepository;

        public OfficeService(IOfficeRepository officeRepository) {
            _officeRepository = officeRepository;
        }
        
        //TODO corregir muestra mal los puestos de atencion

        
        public void CreateOffice(OfficeDTO officeDTO)
        {
            if (officeDTO.Identificator != null)
            {
    
                if (_officeRepository.GetOffice(officeDTO.Identificator) != null) {
                    throw new ArgumentException($"Oficina ya existe");
                }

                
                //Obtengo puestos de atencion del DTO y los paso a objetos de dominio.
                IList<AttentionPlace> attentionPlaces = new List<AttentionPlace>();

                if (officeDTO.AttentionPlaces != null)
                {
                    IList<AttentionPlaceDTO> attentionPlacesDTO = officeDTO.AttentionPlaces;

                    foreach (AttentionPlaceDTO place in attentionPlacesDTO)
                    {
                        AttentionPlace attentionPlace = new AttentionPlace(place.Number, false);
                        attentionPlaces.Add(attentionPlace);
                    }
                }

                //Crear queue
                Office newOffice = new Office(officeDTO.Identificator, null, attentionPlaces);

                _officeRepository.Add(newOffice);

           
            }
            else
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

        }

       
        public void UpdateOffice(OfficeDTO office)
        {

        }

        public void DeleteOffice(string id)
        {

        }

        public OfficeDTO GetOffice(string id)
        {
            if (id != null)
            {
                Office office = this._officeRepository.GetOffice(id);

                //pasaje de objeto de dominio a DTO
                if (office == null)
                {
                    throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
                    
                }
                else
                {
                    
                    IList<AttentionPlaceDTO> attentionPlaceListDTO = new List<AttentionPlaceDTO>();

                    //si tengo puestos, caso contrario mando la lista vacia.
                    if (office.AttentionPlaceList != null)
                    {
                        IList<AttentionPlace> attentionPlaceList = office.AttentionPlaceList;

                        foreach (AttentionPlace place in attentionPlaceList)
                        {
                            attentionPlaceListDTO.Add(new AttentionPlaceDTO(place.Number));
                        }
                    }

                    OfficeDTO officeDTO = new OfficeDTO(id, attentionPlaceListDTO);
                    return officeDTO;
                }
               
            }

             throw new ArgumentNullException();
            
        }

        public IList<OfficeDTO> GetAll()
        {

            //obtengo mi lista de oficinas en memoria.
            IList<Office> offices = this._officeRepository.GetAll();
           
            if (!offices.Any())
            {
                Console.WriteLine("No hay datos en la lista");
                return new List<OfficeDTO>();
            }
            else
            {
                //Armado de la lista de officinas DTO.
                IList<OfficeDTO> officesDTO = new List<OfficeDTO>();
                IList<AttentionPlaceDTO> attentionPlacesDTO = new List<AttentionPlaceDTO>();

                foreach (Office office in offices)
                {
                    if (office.Identificator != null)
                    {
                        if (office.AttentionPlaceList != null)//si no tengo la lista de puestos vacia.
                        {
                            IList<AttentionPlace> attentionPlaces = office.AttentionPlaceList;
                        
                            foreach (AttentionPlace attentionPlace in attentionPlaces)
                            {
                                attentionPlacesDTO.Add(new AttentionPlaceDTO(attentionPlace.Number));
                            }
                            
                        }
                       

                        OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlacesDTO);
                        officesDTO.Add(officeDTO);
                    }

                }

                return officesDTO;
            }
            
        }

    }
}
