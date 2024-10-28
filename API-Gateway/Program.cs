using API_Gateway.Services;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// var monitorClient = builder.AddProject<Projects.Monitor_Client>("monitor-client");

var comercialOffice = builder
    .AddProject<Projects.Commercial_Office>("commercial-office");

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management");

=======
builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddHttpClient<CommercialOfficeService>(static client =>
{
    /*
     * El nombre usado para el servicio fue el que previamente le configuramos en la app host
     */
    client.BaseAddress = new("http://commercial-office");
});
>>>>>>> DOTNET-7-Commercial-Office

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();