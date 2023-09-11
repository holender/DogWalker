using GraphQL;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<WalkerPlanerDbContext>(op => op.UseSqlite("Data Source=walkerPlaner.db"));

builder.Services
    .AddGraphQLServer()
    .AddType<Query>()
    .RegisterDbContext<WalkerPlanerDbContext>(DbContextKind.Pooled);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();
