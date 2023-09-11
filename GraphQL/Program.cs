using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<WalkerPlanerDbContext>(op => op.UseSqlite("Data Source=walkerPlaner.db"));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<WalkerPlanerDbContext>(DbContextKind.Pooled);

builder.Services
    .AddGraphQLServer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();
