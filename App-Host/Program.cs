using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var qmCache = builder.AddRedis("quality-management-cache")
    .WithRedisCommander(containerName: "redis-commander-UI");

var coCache = builder.AddRedis("commercial-office-cache");

var authentication = builder.AddProject<Projects.Authentication>("auth-service");

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management")
    .WithReference(qmCache);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office")
    .WithReference(qualityManagement)
    .WithReference(coCache);

var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway")
    .WithReference(commercialOffice)
    .WithReference(qualityManagement)
    .WithReference(authentication);

authentication.WithReference(apiGateway);

var app = builder.Build();

app.Run();