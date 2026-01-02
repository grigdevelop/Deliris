using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var identity = builder.AddProject<IdentityService_Api>("identity-api");

builder.Build().Run();
