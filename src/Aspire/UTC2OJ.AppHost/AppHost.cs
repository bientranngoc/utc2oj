using Aspire.Hosting;
using Aspire.Hosting.PostgreSQL;
using Aspire.Hosting.RabbitMQ;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add Postgres DBs (mỗi service có DB riêng)
var userDb = builder.AddPostgres("user-db").WithPgAdmin(); // WithPgAdmin cho admin UI local
var problemDb = builder.AddPostgres("problem-db");
var submissionDb = builder.AddPostgres("submission-db");
var executionDb = builder.AddPostgres("execution-db");
var contestDb = builder.AddPostgres("contest-db");

// Add RabbitMQ cho async (submission -> execution)
var rabbitMq = builder.AddRabbitMQ("rabbitmq").WithManagement();

// Add Services (sẽ add project sau khi tạo)
var apiGateway = builder.AddProject<Utc2oj.Services.ApiGateway>("api-gateway");
var userService = builder
    .AddProject<Utc2oj.Services.User.Api>("user-service")
    .WithReference(userDb) // Auto config connection string
    .WithReference(rabbitMq); // Nếu cần queue
var problemService = builder
    .AddProject<Utc2oj.Services.Problem.Api>("problem-service")
    .WithReference(problemDb);
var submissionService = builder
    .AddProject<Utc2oj.Services.Submission.Api>("submission-service")
    .WithReference(submissionDb)
    .WithReference(rabbitMq);
var executionService = builder
    .AddProject<Utc2oj.Services.Execution.Api>("execution-service")
    .WithReference(executionDb)
    .WithReference(rabbitMq);
var contestService = builder
    .AddProject<Utc2oj.Services.Contest.Api>("contest-service")
    .WithReference(contestDb)
    .WithReference(problemDb)
    .WithReference(userDb);

builder.Build().Run();
