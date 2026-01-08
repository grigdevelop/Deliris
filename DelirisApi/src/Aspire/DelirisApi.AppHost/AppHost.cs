using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var identity = builder.AddProject<Deliris_IdentityService_Api>("identity-api");

builder.Build().Run();
