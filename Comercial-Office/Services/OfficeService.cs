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
        

        
        public void CreateOffice(OfficeDTO officeDTO)
        {
            if (officeDTO.Identificator != null)
            {
                var exists = _officeRepository.GetOffice(officeDTO.Identificator);

                if (exists != null) {
                    throw new ArgumentException($"Oficina ya existe");
                }

                //Crear queue
                //Crear una lista de oficinas
                //crear objeto Oficina
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
                if (office.Identificator != null)
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
                        IList<AttentionPlace> attentionPlaces = office.AttentionPlaceList;

                        if (attentionPlaces != null)
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

                return officesDTO;
            }
            
        }

    }
}
