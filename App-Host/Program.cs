using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office");
var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway")
    .WithReference(commercialOffice);

var app = builder.Build();

app.Run();