using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Commercial_Office.Hubs
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class CommercialOfficeHub : Hub
    {
        /*
        * la wawa

        private HubConnection _connection;
        private readonly ILogger<CommercialOfficeHub> _logger;
        public CommercialOfficeHub(ILogger<CommercialOfficeHub> logger)
        {
            this._logger = logger;
            _connection = new HubConnectionBuilder().WithUrl("api-gateway/connection").Build();
            try
            {
                _connection.StartAsync();
                _logger.LogInformation("Conectado al hub de API-Gateway desde CO");
            }
            catch (Exception e)
            {
                _logger.LogInformation("yo, CommercialOfficeHub no me pude conectar a API-Gateway error: " + e.Message.ToString());
            }
            _logger = logger;
        }

        public async Task RefreshMonitor(string userId, long placeNumber, string officeId)
       {
            //_hub.Clients.All.SendAsync("RefreshMonitor", userId.Item, place.Number, officeId);
            _logger.LogInformation("Yo, CommercialOfficeHub, acabo de ser llamado en la función RefreshMonitor con data: " + userId);
            await _connection.SendAsync("Refresh", userId, placeNumber, officeId);
       }

       public async Task eraseInMonitor(long placeNumber, string officeId)
       {
           // _hub.Clients.All.SendAsync("RefreshMonitor", "remove", place.Number, officeId);

           await _connection.SendAsync("RefreshMonitor", "remove", placeNumber, officeId);
       }
                */
    }
}
