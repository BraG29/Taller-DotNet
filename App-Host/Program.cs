using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var qmCache = builder.AddRedis("quality-management-cache", 6379)
    .WithRedisCommander(containerName: "quality-management-cache-UI");

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management")
    .WithReference(qmCache);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office")
    .WithReference(qualityManagement);

var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway")
    .WithReference(commercialOffice)
    .WithReference(qualityManagement);

var app = builder.Build();

app.Run();