using Google.Protobuf.WellKnownTypes;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);


//this one already has the API_Gateway.Client included in it's project file .csproj
var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway");


var qmCache = builder.AddRedis("quality-management-cache")
    .WithRedisCommander(containerName: "redis-commander-UI");

var coCache = builder.AddRedis("commercial-office-cache");


var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management")
    .WithReference(qmCache)
    .WithReference(apiGateway);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office")
    .WithReference(qualityManagement)
    .WithReference(apiGateway)
    .WithReference(coCache);


apiGateway.WithReference(qualityManagement);
apiGateway.WithReference(commercialOffice);



var app = builder.Build();

app.Run();