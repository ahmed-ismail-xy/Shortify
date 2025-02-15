var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Shortify_Api>("shortify-api");

builder.Build().Run();
