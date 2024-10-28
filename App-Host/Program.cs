using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management");

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office").WithReference(qualityManagement);

var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway")
    .WithReference(commercialOffice)
    .WithReference(qualityManagement);

var app = builder.Build();

app.Run();