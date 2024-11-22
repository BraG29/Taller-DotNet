namespace API_Gateway.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Hosting;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ConnectionHub : Hub{

        private HubConnection _connection;
        private readonly ILogger<ConnectionHub> _logger;
        private readonly IHubContext<ConnectionHub> _hub;
        

        public ConnectionHub( ILogger<ConnectionHub> logger, IHubContext<ConnectionHub> hub){
            

            this._logger = logger;
            this._hub = hub;

        }

        #region MonitorClient~

        //this function is called in OfficeService thru SignalR client
        public async Task RefreshMonitor(string userId, long place, string officeId){

            // Acá queremos enviar la señal con la data de CO para que.Client la levante
            this.Clients.All.SendAsync("RefreshMonitorClient" + officeId, userId, place, officeId);

            _logger.LogInformation("Soy RefreshMonitor dentro de ConnectionHub, me acaba de llegar la data para avisarle a ClientMonitor");
            _logger.LogInformation(userId + place + officeId);
        }
        #endregion

        #region RealTime Metrics

        public async Task ClientLeavesTheQueue(string officeId, int[] queue)
        {

            Console.WriteLine("\n\nRecibiendo data y enviando data a ClientLeavesQueue\n\n");

            await Clients.All.SendAsync("ClientLeavesQueue" + officeId, queue);

        }

        public async Task ClientEnterTheQueue(string officeId, int[] queue)
        {

            Console.WriteLine($"\n\nRecibiendo data y enviando data a ClientEnterQueue\n\n {officeId}");

            await Clients.All.SendAsync("ClientEnterQueue" + officeId, queue);

        }

        public async Task PositionReleased(string officeId, int[] amount)
        {

            Console.WriteLine("\n\nRecibiendo data y enviando data a ClientLeavesQueue\n\n");

            await Clients.All.SendAsync("PositionReleased" + officeId, amount);

        }

        #endregion
    }
}
