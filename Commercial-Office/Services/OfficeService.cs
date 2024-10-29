using Commercial_Office.Model;
using Commercial_Office.DTO;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using static Commercial_Office.Model.Office;
using Microsoft.Extensions.Logging;
using Commercial_Office.Hubs;

namespace Commercial_Office.Services 
{
    public class OfficeService : IOfficeService
    {

        private readonly IOfficeRepository _officeRepository;
        private readonly ILogger<OfficeService> _logger;
        private readonly HubService _hubService;
        private readonly IHubContext<CommercialOfficeHub> _hub;
        private readonly QualityManagementService  _qualityManagementService;

        public OfficeService(IOfficeRepository officeRepository, ILogger<OfficeService> logger,
            HubService service, IHubContext<CommercialOfficeHub> hub, QualityManagementService qualityManagementService) {
            _officeRepository = officeRepository;
            _logger = logger;
            _hubService = service;
            _hub = hub;
            _qualityManagementService = qualityManagementService;
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

                        if (place.Number < 0)
                        {
                            throw new ArgumentException($"Numero de puesto no puede ser menor a 0");
                        }

                        ulong placeNumber = (ulong)place.Number;
                        AttentionPlace attentionPlace = new AttentionPlace(placeNumber, false, "0");
                        attentionPlaces.Add(attentionPlace);
                    }
                }

                ConcurrentQueue<TimedQueueItem<string>> queue = new ConcurrentQueue<TimedQueueItem<string>>();

                Office newOffice = new Office(officeDTO.Identificator, queue, attentionPlaces);

                _officeRepository.Add(newOffice);

           
            }
            else
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

        }

       
        public void UpdateOffice(string officeId, IList<AttentionPlaceDTO> placesDTO)
        {
            if (officeId == null || placesDTO == null)
            {
                throw new ArgumentNullException($"Parametro/s vacio/s.");
            }

            Office office = _officeRepository.GetOffice(officeId);

            if(office == null)
            {
                throw new KeyNotFoundException($"No existe la oficina");
            }

            foreach (AttentionPlaceDTO placeDTO in placesDTO)
            {
                if (placeDTO.Number < 0)
                {
                    throw new ArgumentException($"No se permiten numeros menores a 0");
                }

                try
                {
                    //actualizo lugares existentes
                    AttentionPlace place = office.AttentionPlaceList
                        .First(p => p.Number == (ulong)placeDTO.Number);

                    if (place != null)
                    {
                        place.IsAvailable = placeDTO.Available;
                    }
                    else
                    {
                        //si no existe agrego a la lista
                        office.AttentionPlaceList.Add(new AttentionPlace((ulong)placeDTO.Number, placeDTO.Available, "0"));
                    }
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException($"No existe el puesto");
                }
           
            }

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

            TimedQueueItem<string> user = new TimedQueueItem<string>(userId);
            office.UserQueue.Enqueue(user);
        }

        public async Task CallNextUser(string officeId, long placeNumber)
        {
            if (officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos (no pueden ser menores a 0) o vacios");
            }

            if (placeNumber < 0)
            {
                throw new ArgumentException($"No se aceptan números menores a 0");
            }

            Office office = this._officeRepository.GetOffice(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            ulong placeNumberCast = (ulong)placeNumber;

            try
            {
                //obtener el puesto que coincida con el numero
                AttentionPlace place = office.AttentionPlaceList
                    .First(place => place.Number == placeNumberCast);

                if (!place.IsAvailable)//si el puesto esta ocupado
                {
                    throw new ArgumentException($"El puesto esta ocupado");
                }

                //Si hay usuarios en la queue saco uno para ocupar el puesto desde el que lo llaman
                if (office.UserQueue.TryDequeue(out TimedQueueItem<string>? userId))
                {
                    place.IsAvailable = false; //ocupo el puesto


                    //llamo endpoint que me devuelve id de tramite y seteo el atributo ProcessId del lugar
                    string procedureId = await _qualityManagementService.StartProcedure(officeId, placeNumber, DateTime.UtcNow);

                    if (procedureId == null)
                    {
                        throw new ArgumentNullException($"Identificadores de tramite vacio");
                    }

                    place.ProcedureId = procedureId;

                    //TODO: Consultar
                    //desde Apigateway
                    _hub.Clients.All.SendAsync("RefreshMonitor", userId.Item, place.Number, officeId);

                }
                else
                {
                    throw new KeyNotFoundException($"No hay usuarios en la cola");
                }

            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"No existe el puesto");
            }

        }


        public async Task ReleasePosition(string officeId, long placeNumber)
        {

            if (officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos (no pueden ser menores a 0) o vacios");
            }

            if(placeNumber < 0  )
            {
                throw new ArgumentException($"No se aceptan números menores a 0");
            }

            Office office = this._officeRepository.GetOffice(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            ulong placeNumberCast = (ulong)placeNumber;

            try
            {
                //obtener el puesto que coincida con el numero
                AttentionPlace place = office.AttentionPlaceList
                    .First(place => place.Number == placeNumberCast);

                if (!place.IsAvailable)//si el puesto esta ocupado
                {

                    place.IsAvailable = true; //libero el puesto

                    //llamada a endpoint de qualityManagement para finalizar tramite
                    var task = _qualityManagementService.FinishProcedure(place.ProcedureId, DateTime.UtcNow);

                    //TODO: Consultar
                    //desde Apigateway

                    _hub.Clients.All.SendAsync("RefreshMonitor", "remove", place.Number, officeId); 

                    await task;
                }
                else //Si el puesto ya se encuentra libre
                {
                    throw new ArgumentException($"El puesto ya se encuentra liberado");
                }
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"No existe el puesto");
            }

        }




        //FUNCIONES DE METRICAS
        //Obtener cant usuarios en espera
        //Promedio de espera
        //fecha de consultas

    }
}
