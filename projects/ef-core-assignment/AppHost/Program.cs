using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("mssql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<EfAssignment>("assignment-one-api")
    .WithExplicitStart()
    .WithReference(sqlServer.AddDatabase("assignment-one-db"));

builder.Build().Run();