using Commercial_Office.Model;
using Commercial_Office.DTO;
using System.Collections.Concurrent;
using Commercial_Office.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Commercial_Office.Services 
{
    public class OfficeService : IOfficeService
    {

        private readonly IOfficeRepository _officeRepository;
        private readonly ILogger<OfficeService> _logger;
        private readonly IHubContext<CommercialOfficeHub> _hub;
        
        public OfficeService(IOfficeRepository officeRepository, ILogger<OfficeService> logger,
            IHubContext<CommercialOfficeHub> hub) {
            _officeRepository = officeRepository;
            _logger = logger;
            _hub = hub;

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
                        ulong placeNumber = (ulong)place.Number;
                        AttentionPlace attentionPlace = new AttentionPlace(placeNumber, false);
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
                            long placeNumber = (long)place.Number;
                            attentionPlaceListDTO.Add(new AttentionPlaceDTO(placeNumber, place.IsAvailable));
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
                                long attentionPlaceNumber = (long)attentionPlace.Number;
                                attentionPlacesDTO.Add(new AttentionPlaceDTO(attentionPlaceNumber, attentionPlace.IsAvailable));
                            }
                            
                        }
                       
                        OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlacesDTO);
                        officesDTO.Add(officeDTO);
                    }

                }

                return officesDTO;
            }
            
        }



        public async void RegisterUser(string userId, string officeId)
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

            //Esto me retorna un numero: busca el primer puesto libre de la lista de puestos de la oficina
            var post = office.IsAvailable();

            if (post == -1)
            {
                //no hay puesto disponible coloco al cliente en la queue
                office.UserQueue.Enqueue(userId);
                Console.WriteLine("Usuario entra a la queue");
            }
            else
            {
                /*
                 * TODO: Cambiar el llamado de "All" a "AllExcept" para no enviar los datos al servicio de QM cuando este implementado
                 */
                _hub.Clients.All.SendAsync("RefreshMonitor", userId, post, officeId);
                office.OcupyAttentionPlace((ulong)post);
            }

        }

        public void ReleasePosition(string officeId, long placeNumber)
        {

            if (placeNumber < 0 || officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos (no pueden ser menores a 0) o vacios");
            }

            Office office = this._officeRepository.GetOffice(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            try
            {
                ulong placeNumberCast = (ulong)placeNumber;

                //obtener el puesto que coincida con el numero
                AttentionPlace place = office.AttentionPlaceList
                    .First(place => place.Number == placeNumberCast);

                if (!place.IsAvailable)//si el puesto esta ocupado
                {
                    place.IsAvailable = true; //libero el puesto

                    //Si hay usuarios disponibles en la cola saco uno y ocupo el puesto con ese usuario.
                    if (office.UserQueue.TryDequeue(out string? userId))
                    {
                        //TODO llamar la hub y tirar la data de puesto ocupado con usuario de la queue.
                        office.OcupyAttentionPlace(place.Number);
                    }
                }
                else //Si el puesto ya se encuentra libre
                {
                    throw new ArgumentException("El puesto ya se encuentra liberado");
                }
                
            }
            catch (KeyNotFoundException) //si no encuentro el puesto
            {
                throw new KeyNotFoundException($"No existe el puesto.");
            }
        }
    }
}
