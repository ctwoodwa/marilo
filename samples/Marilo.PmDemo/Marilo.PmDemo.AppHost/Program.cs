var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure resources
var postgresServer = builder.AddPostgres("pmdemodb-server")
    .WithDataVolume();
var postgres = postgresServer.AddDatabase("pmdemodb");

var redis = builder.AddRedis("pmdemo-redis");

var rabbit = builder.AddRabbitMQ("pmdemo-rabbit")
    .WithManagementPlugin();

// Mock OIDC dependency
var okta = builder.AddProject<Projects.MockOktaService>("mock-okta");

// Data API Builder — exposes the Postgres schema as GraphQL.
// dab-config.json lives next to the .slnx and is bind-mounted into the container.
// DAB reads its connection string from the DAB_CONNECTION_STRING env var (see dab-config.json).
var dab = builder.AddContainer("pmdemo-dab", "mcr.microsoft.com/azure-databases/data-api-builder", "latest")
    .WithBindMount("../dab-config.json", "/App/dab-config.json", isReadOnly: true)
    .WithEnvironment("DAB_CONNECTION_STRING", postgres.Resource.ConnectionStringExpression)
    .WithHttpEndpoint(targetPort: 5000, name: "graphql")
    .WaitFor(postgres);

// Server project
builder.AddProject<Projects.Marilo_PmDemo>("pmdemo-web")
    .WithReference(postgres)
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(okta)
    .WithEnvironment("DAB_GRAPHQL_URL", dab.GetEndpoint("graphql"))
    .WaitFor(postgres)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(dab);

builder.Build().Run();
