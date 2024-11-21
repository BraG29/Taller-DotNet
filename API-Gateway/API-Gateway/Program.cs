using System.Text;
using API_Gateway.Client.Pages;
using API_Gateway.Components;
using API_Gateway.Hub;
using API_Gateway.Services;
using Microsoft.IdentityModel.Tokens;
using Radzen;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

//a�adimos que vamos a utilizar SignalR
builder.Services.AddSignalR();

//hacemos singleton el ConnectionHub
builder.Services.AddSingleton<ConnectionHub>();

builder.Services.AddHttpClient<CommercialOfficeService>(static client =>
{
    /*
 * El nombre usado para el servicio fue el que previamente le configuramos en la app host
 */
    client.BaseAddress = new("http://commercial-office");
});

builder.Services.AddHttpClient<QualityManagementService>(static client =>
{
    client.BaseAddress = new("http://quality-management");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//indicamos que con esta URL se podr�a conectar al HUB.
app.MapHub<ConnectionHub>("/connection");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();