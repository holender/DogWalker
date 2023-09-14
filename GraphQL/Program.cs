using GraphQL.Configuration;
using GraphQL.Extensions;
using HotChocolate.AspNetCore;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddCors();

builder.Services.AddPooledWalkerPlanerDbContext();

builder.Services
    .AddGraphQLServer()
    .AddTypes()
    .AddQueryType()
    .AddMutationType()
    .AddSubscriptionType()
    .AddUploadType()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions()
    .AllowIntrospection(false)
    .AddHttpRequestInterceptor<IntrospectionInterceptor>()
    .AddGlobalObjectIdentification()
    .RegisterDbContext<WalkerPlanerDbContext>()    
    .ModifyOptions(o => o.EnableDefer = true);

var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseWebSockets();

app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    EnableSchemaRequests = true,
    AllowedGetOperations = AllowedGetOperations.All,
    Tool = {
        Enable = env.IsDevelopment()
    }
    
});

//app.MapBananaCakePop("/graphql/ui");
//app.MapGraphQLHttp("/graphql/http");
//app.MapGraphQLWebSocket("/graphql/ws");
//app.MapGraphQLSchema("/graphql/schema");

app.MapGraphQLVoyager();
app.Run();
