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

builder.Services
   .AddDbContextPool<WalkerPlanerDbContext>(op => op.UseSqlite("Data Source=walkerPlaner.db"));

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
    .AddFiltering()
    .AddSorting()
    .AddGlobalObjectIdentification()
    .AddDataLoader<IWalkerByIdDataLoader, WalkerByIdDataLoader>()
    .AddDataLoader<ISessionByIdDataLoader, SessionByIdDataLoader>()
    .AddDataLoader<IDogByIdDataLoader, DogByIdDataLoader>()
    .AddDataLoader<TrackByIdDataLoader>()
    .RegisterDbContext<WalkerPlanerDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();
