using Commercial_Office.Model;
using Commercial_Office.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Commercial_Office.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Commercial_Office.Services.Interfaces;
 
namespace Commercial_Office.Services.Implementations
{
    //here we make a quick static class to properly configure the IHubConnectionBuilder for a SignalR environment
    public static class HubConnectionExtensions{

        //the function in question that does the configuration
        public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder builder, string url, IHttpMessageHandlerFactory clientFactory)
        {
            return builder.WithUrl(url, options =>
            {
                options.HttpMessageHandlerFactory = _ => clientFactory.CreateHandler();
            });
        }
    }


        public class OfficeService : IOfficeService{

        private readonly IOfficeRepository _officeRepository;
        private readonly ILogger<OfficeService> _logger;
        private readonly QualityManagementService  _qualityManagementService;
        private readonly HubConnection _hubApiGatewayConnection;
        private readonly IOfficeQueueService _queueService;
        private readonly IRedisService _redisService;


        public OfficeService(IOfficeRepository officeRepository,
                            ILogger<OfficeService> logger,
                            QualityManagementService qualityManagementService,
                            IOfficeQueueService queueService,
                            IHttpMessageHandlerFactory msgFactory,
                            IRedisService redisService)
        {
            _officeRepository = officeRepository;
            _logger = logger;
            _qualityManagementService = qualityManagementService;

            var hubConnection = new HubConnectionBuilder();

            //now we just use the static configuration function that David Fowler wrote above
            HubConnectionExtensions.WithUrl(hubConnection, "http://api-gateway/connection", msgFactory);

            //and we build the connection
            _hubApiGatewayConnection = hubConnection.Build();

            _queueService = queueService;
            _redisService = redisService;
        }

        public async Task CreateOffice(OfficeDTO officeDTO)
        {
            /*
             * llamar a un async := o
             *  -----
             * -----
             * leo o
             *
             */

            if (officeDTO.Identificator != null)
            {
                if (await _officeRepository.GetOffice(officeDTO.Identificator) != null)
                {
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

                        // AttentionPlace attentionPlace = new AttentionPlace(0, place.Number, true, 0, officeDTO.Identificator);
                        AttentionPlace attentionPlace = new AttentionPlace(0, place.Number, true);
                        attentionPlaces.Add(attentionPlace);
                    }
                }

                Office newOffice = new Office(officeDTO.Identificator, attentionPlaces);

                try
                {
                    await _officeRepository.Add(newOffice);
                }
                catch (DbUpdateException ex)
                {

                    throw new DbUpdateException($"Error: " + ex.ToString());
                }
            }
            else
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

        }

        public async Task UpdateOffice(string officeId, IList<AttentionPlaceDTO> placesDTO)
        {
            if (officeId == null || placesDTO == null)
            {
                throw new ArgumentNullException($"Parametro/s vacio/s.");
            }

            Office office = await _officeRepository.GetOffice(officeId);

            if (office == null)
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
                        .First(p => p.Number == placeDTO.Number);

                    if (place != null)
                    {
                        place.IsAvailable = placeDTO.Available;
                    }
                    else
                    {
                        //LLamaria a AddDB
                        // office.AttentionPlaceList.Add(new AttentionPlace(0, placeDTO.Number, placeDTO.Available, 0, officeId));
                        office.AttentionPlaceList.Add(new AttentionPlace(0, placeDTO.Number, placeDTO.Available));

                        await _officeRepository.Update(office);

                    }
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException($"No existe el puesto");
                }

            }

        }

        public async Task DeleteOffice(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"Identificador no puede ser vacio.");
            }
            var office = await _officeRepository.GetOffice(id);
            if (office == null)
            {
                throw new KeyNotFoundException($"No existe una oficina con ese identificador");
            }

            try
            {
                await _officeRepository.Delete(office);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<OfficeDTO> GetOffice(string id)
        {
            _logger.LogInformation("Estoy en en obtener una oficina");

            if (id != null)
            {
                Office office = await _officeRepository.GetOffice(id);

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
                        //Transfromo cada  attentionPlace en attentionPlaceDTO
                        attentionPlaceListDTO = office.AttentionPlaceList.Select(place =>
                            new AttentionPlaceDTO(place.Number, place.IsAvailable)
                            ).ToList();

                    }

                    OfficeDTO officeDTO = new OfficeDTO(id, attentionPlaceListDTO);
                    return officeDTO;
                }

            }

            throw new ArgumentNullException();

        }

        public async Task<IList<OfficeDTO>> GetAll()
        {

            //obtengo mi lista de oficinas en memoria.
            IList<Office> offices = await _officeRepository.GetAll();

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

                        IList<AttentionPlaceDTO> attentionPlaceListDTO = new List<AttentionPlaceDTO>();

                        if (office.AttentionPlaceList != null)
                        {
                            //Transfromo cada  attentionPlace en attentionPlaceDTO
                            attentionPlaceListDTO = office.AttentionPlaceList.Select(place =>
                            new AttentionPlaceDTO(place.Number, place.IsAvailable)
                            ).ToList();

                        }

                        OfficeDTO officeDTO = new OfficeDTO(office.Identificator, attentionPlaceListDTO);
                        officesDTO.Add(officeDTO);
                    }

                }

                return officesDTO;
            }

        }

        public async Task RegisterUser(string userId, string officeId)
        {

            if (userId == null || officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos o vacios");
            }

            if (await GetOfficeIfExists(officeId) == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            var queue = _queueService.GetQueueOrCreateQueue(officeId);

            queue.Enqueue(new TimedQueueItem<string>(userId));

            _qualityManagementService.CallClientRegistration(officeId);
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

            Office? office = await GetOfficeIfExists(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }


            try
            {
                //obtener el puesto que coincida con el numero
                AttentionPlace place = office.AttentionPlaceList
                    .First(place => place.Number == placeNumber);

                if (!place.IsAvailable) //si el puesto esta ocupado
                {
                    throw new ArgumentException($"El puesto esta ocupado");
                }

                var queue = _queueService.GetQueueOrCreateQueue(officeId);
                //Si hay usuarios en la queue saco uno para ocupar el puesto desde el que lo llaman
                if (queue.TryDequeue(out TimedQueueItem<string>? userId))
                {
                    DateTime dequeuedTime = DateTime.Now;
                    DateTime enqueuedTime = userId.EnqueuedTime; //pasar tiempo de espera

                    place.IsAvailable = false; //ocupo el puesto

                    TimeSpan waitTime = dequeuedTime - enqueuedTime;

                    Console.WriteLine("Tiempo de espera: " + waitTime.ToString());

                    string timeDifferenceString = waitTime.ToString(@"hh\:mm\:ss");

                    Console.WriteLine("Tiempo de espera en string: " + timeDifferenceString);

                    string procedureIdString = await _qualityManagementService.StartProcedure(officeId, placeNumber, DateTime.Now, timeDifferenceString);

                    if (procedureIdString == null)
                    {
                        throw new ArgumentNullException($"Identificadores de tramite vacio");
                    }

                    long procedureId = long.Parse(procedureIdString);

                    place.ProcedureId = procedureId;

                    await _redisService.SetValueAsync(officeId, office);

                    try
                    {//if the conecction to SignalR has been suspended/killed

                        if (_hubApiGatewayConnection.State != HubConnectionState.Connected)
                        {

                            await _hubApiGatewayConnection.StartAsync();//try to reconenct
                            await _hubApiGatewayConnection.InvokeAsync("RefreshMonitor", userId.Item, place.Number, officeId);//we send the data
                        }
                        else
                        {
                            await _hubApiGatewayConnection.InvokeAsync("RefreshMonitor", userId.Item, place.Number, officeId);//we send data, without reconnecting
                        }

                    }
                    catch (Exception e)
                    {

                        _logger.LogInformation("YO SOY EL OfficeService y acabo de fallar al pasarla la data por SignalR client a HubConnections" + e.Message.ToString());
                    }

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
            catch (NullReferenceException e)
            {
                Console.WriteLine("El Redis devolvio un valor nulo");
            }

        }


        public async Task ReleasePosition(string officeId, long placeNumber)
        {

            if (officeId == null)
            {
                throw new ArgumentNullException($"Identificadores invalidos (no pueden ser menores a 0) o vacios");
            }

            if (placeNumber < 0)
            {
                throw new ArgumentException($"No se aceptan números menores a 0");
            }
            
            Office? office = await GetOfficeIfExists(officeId);
            if (office == null)
            {
                throw new KeyNotFoundException($"No hay una oficina con ese identificador.");
            }

            try
            {
                
                //obtener el puesto que coincida con el numero
                AttentionPlace place = office.AttentionPlaceList
                    .First(place => place.Number == placeNumber);

                if (!place.IsAvailable)//si el puesto esta ocupado
                {

                    place.IsAvailable = true; //libero el puesto

                    //le paso solo fecha el id lo maneja el servicio al que llamo
                    Console.WriteLine("Antes de  tirarle al servicio");
                    await _qualityManagementService.FinishProcedure(place.ProcedureId, DateTime.Now);

                   

                    place.ProcedureId = 0;
                    
                    await _redisService.SetValueAsync(officeId, office);

                    try
                    {//if the conecction to SignalR has been suspended/killed

                        if (_hubApiGatewayConnection.State != HubConnectionState.Connected)
                        {
                            await _hubApiGatewayConnection.StartAsync();//try to reconenct
                            await _hubApiGatewayConnection.InvokeAsync("RefreshMonitor", "remove", place.Number, officeId);
                        }
                        else
                        {
                            await _hubApiGatewayConnection.InvokeAsync("RefreshMonitor", "remove", place.Number, officeId);
                        }

                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation("YO SOY EL OfficeService y acabo de fallar al pasarla la data por SignalR client a HubConnections" + e.Message.ToString());
                    }
                }
                else //Si el puesto ya se encuentra libre
                {
                    throw new ArgumentException($"El puesto ya se encuentra liberado");
                }
            }
            catch (InvalidOperationException ex)
            {
               throw new InvalidOperationException($"No existe el puesto" + ex.ToString());
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("El Redis devolvio un valor nulo");
            }


        }

        private async Task<Office?> GetOfficeIfExists(string officeId)
        {
            Office? office = await _redisService.GetValueAsync(officeId);

            if (office != null) return office;
            
            Console.WriteLine("-----[LA OFICINA NO EXISTE EN EL REDIS]-----");
            
            if (!await _officeRepository.ExistById(officeId)) return null;
            
            Console.WriteLine("-----[LA OFICINA EXISTE EN EL REPO PERO NO EN EL REDIS]-----");
            
            office = await _officeRepository.GetOffice(officeId);
            await _redisService.SetValueAsync(officeId, office);
            
            return office;

        }

    }
}
