using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Types.Sessions;
using GraphQL.Types.Tracks;
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
    .AddQueryType(d => d.Name(OperationTypeNames.Query))
        .AddTypeExtension<WalkerQueries>()
        .AddTypeExtension<SessionQueries>()
        .AddTypeExtension<TrackQueries>()
    .AddMutationType(d => d.Name(OperationTypeNames.Mutation))
    .AddTypeExtension<SessionMutations>()
    .AddTypeExtension<WalkerMutations>()
    .AddTypeExtension<TrackMutations>()
    .AddType<DogType>()
    .AddType<SessionType>()
    .AddType<WalkerType>()
    .AddType<TrackType>()
    .AddGlobalObjectIdentification()
    .AddDataLoader<WalkerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>()
    .RegisterDbContext<WalkerPlanerDbContext>(DbContextKind.Pooled);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();
