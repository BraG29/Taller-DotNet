using Commercial_Office.Controllers;
using Commercial_Office.Infraestructure;
using Commercial_Office.Model;
using Commercial_Office.Services;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//servicio signalR
builder.Services.AddSignalR();

//añadir controler singleton
builder.Services.AddSingleton<IOfficeRepository, OfficeRepositoryImpl>();
builder.Services.AddSingleton<IOfficeService, OfficeService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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
/*
app.MapGet("/", () =>
{
    return "Hola Mundo";
});*/

app.Run();
