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

    //Register Types
    .AddTypes()
    .AddQueryType()
    .AddMutationType()
    .AddSubscriptionType()

    //Adding Filtering and Sorting
    .AddFiltering()
    .AddSorting()

    //Subscriptions
    //.AddMutationConventions()
    .AddInMemorySubscriptions()

    //Introspection
    .AddIntrospectionAllowedRule()
    .AllowIntrospection(false)
    .AddHttpRequestInterceptor<IntrospectionInterceptor>()

    //Relay
    .AddGlobalObjectIdentification()

    //Db Context
    .RegisterDbContext<WalkerPlanerDbContext>()    
    .ModifyOptions(o => o.EnableDefer = true);

var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseWebSockets();

app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    EnableSchemaRequests = false,
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
