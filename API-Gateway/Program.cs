using k8s.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = DistributedApplication.CreateBuilder(args);

var monitorClient = builder.AddProject<Projects.Monitor_Client>("monitor-client");
var comercialOffice = builder.AddProject<Projects.Commercial_Office>("commercial-office");

var app = builder.Build();

app.Run();