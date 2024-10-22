var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Web_Page_ApiService>("apiservice");

builder.AddProject<Projects.Web_Page_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
