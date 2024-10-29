using API_Gateway.Services;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddHttpClient<CommercialOfficeService>(static client =>
{
    /*
     * El nombre usado para el servicio fue el que previamente le configuramos en la app host
     */
    client.BaseAddress = new("http://commercial-office");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();