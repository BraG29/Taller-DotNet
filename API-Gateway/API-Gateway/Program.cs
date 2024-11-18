using API_Gateway.Client.Pages;
using API_Gateway.Components;
using API_Gateway.Hub;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

//añadimos que vamos a utilizar SignalR
builder.Services.AddSignalR();


builder.Services.AddHttpClient<ConnectionHub>(static client =>
{
    /*
     * El nombre usado para el servicio fue el que previamente le configuramos en la app host
     */
    client.BaseAddress = new("http://commercial-office");
});

//hacemos singleton el ConnectionHub
builder.Services.AddSingleton<ConnectionHub>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//indicamos que con esta URL se podría conectar al HUB.
app.MapHub<ConnectionHub>("/connection");
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();