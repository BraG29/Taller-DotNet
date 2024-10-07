using Comercial_Office.Controllers;
using Comercial_Office.Infraestructure;
using Comercial_Office.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//servicio signalR
builder.Services.AddSignalR();

//añadir controler singleton
builder.Services.AddSingleton<IOfficeRepository, OfficeRepositroyImpl>();

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


//TODO
/*
Habilitar documentacion XML para swagger.
Utilizar Optional.
Crear queue al crear oficina.
Implementar conexion con signalR hacia hub de ApiGateway
Testear operaciones del controlador (crear y eliminar)
Pulir DTOS
 */