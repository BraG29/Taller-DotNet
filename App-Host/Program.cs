using Google.Protobuf.WellKnownTypes;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

<<<<<<< HEAD



//this one already has the API_Gateway.Client included in it's project file .csproj
var apiGateway = builder.AddProject<Projects.API_Gateway>("api-gateway");

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management")
    .WithReference(apiGateway);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office")
    .WithReference(qualityManagement)
    .WithReference(apiGateway);
=======
var qmCache = builder.AddRedis("quality-management-cache", 6379)
    .WithRedisCommander(containerName: "quality-management-cache-UI");

var qualityManagement = builder
    .AddProject<Projects.Quality_Management>("quality-management")
    .WithReference(qmCache);

var commercialOffice = builder.AddProject<Commercial_Office>("commercial-office")
    .WithReference(qualityManagement);
>>>>>>> f96acf85cdbd7ad9ba91ae2892649de0dcf0c3f1

apiGateway.WithReference(qualityManagement);
apiGateway.WithReference(commercialOffice);
// this is the one that gives me problems when I execute the Aspire application, saying the following error:
// Error: Cannot find runtime config at C:\Users\shodyWindows\Documents\GitHub\Taller-DotNet\API-Gateway\API-Gateway.Client\bin\Debug\net8.0\API-Gateway.Client.runtimeconfig.json
//var clientApiGateway = builder.AddProject<Projects.API_Gateway_Client>("api-gateway-client")
//    .WithReference(commercialOffice);

var app = builder.Build();

app.Run();