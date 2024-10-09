using Microsoft.AspNetCore.SignalR.Client;
namespace Comercial_Office.Controllers

{
    public class SignalRService
    {
        private HubConnection _connection;

        public async Task connect()
        {
            //construir conexion
            _connection = new HubConnectionBuilder().WithUrl("urlTest.com").Build();

            await _connection.StartAsync();
            Console.WriteLine("Conexion establecida con el hub");

            //alguna funcion de enviar un mensaje
        }
    }
}
