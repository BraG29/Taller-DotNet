using Commercial_Office.Controllers;
using Commercial_Office.Infraestructure;
using Commercial_Office.Model;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Commercial_Office.Hubs;
using ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Commercial_Office.DataAccess;
using Commercial_Office.Services.Implementations;
using Commercial_Office.Services.Interfaces;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.

//MemoryCache 
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<QualityManagementService>(static client =>
{
    /*
     * El nombre usado para el servicio fue el que previamente le configuramos en la app host
     */
    client.BaseAddress = new("http://quality-management");
});

//servicio signalR
builder.Services.AddSignalR();

//Logger de Serilog
builder.Host.UseSerilog((hostBuilderCtx, loggerConf) =>
{
    loggerConf.WriteTo.Console()
        .WriteTo.Debug()
        .ReadFrom.Configuration(hostBuilderCtx.Configuration);
});



builder.Services.AddScoped<IOfficeRepository, OfficeRepositoryImpl>();
builder.Services.AddScoped<IOfficeService, OfficeService>();

builder.Services.AddSingleton<IOfficeQueueService, OfficeQueueService>();
builder.Services.AddSingleton<CommercialOfficeHub>();
builder.Services.AddSingleton<HubService>();



builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("CODatabase");

builder.Services.AddDbContext<CommercialOfficeDbContext>(options => options.UseSqlServer(connectionString));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Office API",
        Description = "Una API en ASP.NET Core Web para gestionar una oficina",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; ;
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(myAllowSpecificOrigins);
app.MapHub<CommercialOfficeHub>("/commercial-office/hub");

app.Run();
