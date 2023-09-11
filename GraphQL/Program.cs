using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Types.Walkers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<WalkerPlanerDbContext>(op => op.UseSqlite("Data Source=walkerPlaner.db"));

//builder.Services
//    .AddDbContext<WalkerPlanerDbContext>(op => op.UseSqlite("Data Source=walkerPlaner.db"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<WalkerType>()
    .AddDataLoader<WalkerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>()
    .RegisterDbContext<WalkerPlanerDbContext>(DbContextKind.Pooled);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();
