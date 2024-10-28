using Commercial_Office.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
namespace Commercial_Office.Services
{
    public class HubService 
    {
        /*
         * Esto deberia comunicarse con apiGateway en lugar de clientMonitor
         * 
        private HubConnection _connection;

        public HubService()
        {
            _connection = new HubConnectionBuilder().WithUrl("clientMonitor").Build();
        }

        public async Task ConnectToHub()
        {
            await _connection.StartAsync();
            Console.WriteLine("Conectado al hub");
        }
        public async Task refreshMonitor(string userId, long placeNumber, string officeId)
        {
            //_hub.Clients.All.SendAsync("RefreshMonitor", userId.Item, place.Number, officeId);

            await _connection.SendAsync("RefreshMonitor", userId, placeNumber, officeId);
        }

        public async Task eraseInMonitor(long placeNumber, string officeId)
        {
            // _hub.Clients.All.SendAsync("RefreshMonitor", "remove", place.Number, officeId);

            await _connection.SendAsync("RefreshMonitor", "remove", placeNumber, officeId);
        }

        */
    }
}
