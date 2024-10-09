using Microsoft.AspNetCore.SignalR.Client;
namespace Comercial_Office.Services

{
    public class SignalRService
    {
        private HubConnection _connection;

        public async Task connect()
        {

            _connection = new HubConnectionBuilder().WithUrl("urlTest.com").Build();
            await _connection.StartAsync();
            Console.WriteLine("Conexion establecida con el hub");

        }

        public void message()
        {
            Console.WriteLine("Enviar data a un  hub W.I.P");
        }


    }
}
