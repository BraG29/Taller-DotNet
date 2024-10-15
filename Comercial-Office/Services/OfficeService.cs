using Comercial_Office.Model;
using Comercial_Office.DTO;
using System.Collections.Concurrent;

namespace Comercial_Office.Services 
{
    public class OfficeService : IOfficeService
    {

        private readonly IOfficeRepository _officeRepository;
        private readonly ILogger<OfficeService> _logger;

        public OfficeService(IOfficeRepository officeRepository, ILogger<OfficeService> logger) {
            _officeRepository = officeRepository;
            _logger = logger;
        }
        

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

                
                ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

                Office newOffice = new Office(officeDTO.Identificator, queue, attentionPlaces);

                _officeRepository.Add(newOffice);

           
            }
            else
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

        }

       
        public void UpdateOffice(OfficeDTO office)
        {
            //TODO implementar, solo modifica puestos
        }

        public void DeleteOffice(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"Identificador no puede ser vacio.");
            }

            if (_officeRepository.GetOffice(id) == null)
            {
                throw new KeyNotFoundException($"No existe una oficina con ese identificador");
            }

            try
            {
                _officeRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public OfficeDTO GetOffice(string id)
        {
            _logger.LogInformation("Estoy en en obtener una oficina");

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
                            attentionPlaceListDTO.Add(new AttentionPlaceDTO(place.Number, place.IsAvailable));
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

                foreach (Office office in offices)
                {
                    if (office.Identificator != null)
                    {
                        
                        IList<AttentionPlaceDTO> attentionPlacesDTO = new List<AttentionPlaceDTO>();
                        
                        if (office.AttentionPlaceList != null)//si no tengo la lista de puestos vacia.
                        {
                            IList<AttentionPlace> attentionPlaces = office.AttentionPlaceList;
                        
                            foreach (AttentionPlace attentionPlace in attentionPlaces)
                            {
                                attentionPlacesDTO.Add(new AttentionPlaceDTO(attentionPlace.Number, attentionPlace.IsAvailable));
                            }
                            
                        }
                       
                        OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlacesDTO);
                        officesDTO.Add(officeDTO);
                    }

                }

                return officesDTO;
            }
            
        }



        public void RegisterUser(string userId, string officeId)
        {
            if (userId == null || officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos o vacios");
            }

            Office office = this._officeRepository.GetOffice(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            var post = office.IsAvailable();

            //VER COMO CAMBIAR ESTO
            if (post == 0)
            {
                //no hay puesto disponible coloco al cliente en la queue
                office.UserQueue.Enqueue(userId);
                Console.WriteLine("Usuario entra a la queue");
            }
            else
            {
                //llamar la hub y tirar la data.
                Console.WriteLine("Aca estaría llamando al hub para avisar que se ocupo un puesto");
                Console.WriteLine(userId);
                Console.WriteLine(post);
                office.OcupyAttentionPlace(post);
            }

        }

        public void ReleasePosition(string officeId, string placeNumber)
        {
            if (placeNumber == null || officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos o vacios");
            }

            Office office = this._officeRepository.GetOffice(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }



        }
    }
}
