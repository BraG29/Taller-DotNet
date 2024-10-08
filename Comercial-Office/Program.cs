using Comercial_Office.Controllers;
using Comercial_Office.Infraestructure;
using Comercial_Office.Model;
using Comercial_Office.Services;

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
builder.Services.AddSwaggerGen();

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

app.Run();
