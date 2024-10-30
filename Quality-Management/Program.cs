using Quality_Management.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quality_Management.Model;
using Quality_Management.Infraestructure;
using Quality_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProcedureRepository, ProcedureRepositoryImpl>();
builder.Services.AddScoped<IProcedureService, ProcedureServiceImpl>();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("QMDatabase");

builder.Services.AddDbContext<QualityManagementDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();