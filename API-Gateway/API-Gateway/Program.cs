using API_Gateway.Client.Pages;
using API_Gateway.Components;
<<<<<<< HEAD
using API_Gateway.Hub;
=======
using API_Gateway.Services;
>>>>>>> f96acf85cdbd7ad9ba91ae2892649de0dcf0c3f1
using Radzen;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

<<<<<<< HEAD
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


=======
builder.Services.AddHttpClient<CommercialOfficeService>(static client =>
{
    client.BaseAddress = new("http://commercial-office");
});

builder.Services.AddHttpClient<QualityManagementService>(static client =>
{
    client.BaseAddress = new("http://quality-management");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
>>>>>>> f96acf85cdbd7ad9ba91ae2892649de0dcf0c3f1

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

//indicamos que con esta URL se podría conectar al HUB.
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