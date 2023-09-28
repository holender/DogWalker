using GraphQL.Configuration;
using GraphQL.Extensions;
using GraphQL.Services;
using HotChocolate.AspNetCore;
using HotChocolate.Resolvers;
using Infrastructure;
using Infrastructure.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddCors();

using var host = Host.CreateApplicationBuilder(args).Build();
var config = host.Services.GetRequiredService<IConfiguration>();

builder.Services.AddPooledWalkerPlanerDbContext();
builder.Services.AddJwtBearerAuthentication(config);

builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization(c =>
        {
            c.AddPolicy("READ_SESSION",
                b =>
                {
                    b.RequireAssertion(async context =>
                        {
                            if (context.Resource is IMiddlewareContext ctx &&
                                ctx.Parent<Walker>() is { } walkerDb &&
                                ctx.GetUser() is { } user &&
                                walkerDb.Name != null &&
                                walkerDb.Name.Contains(user.FindFirstValue(ClaimTypes.NameIdentifier)!))
                            {
                                return true;
                            }

                            return false;
                        }
                    );
                });

            c.AddPolicy("READ_ADMIN",
                b =>
                {
                    b.RequireRole("Admin");
                });
        })
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
    .AddHttpRequestInterceptor<HttpRequestInterceptor>()
    //.AddSocketSessionInterceptor<SocketSessionInterceptor>()

    //Relay
    .AddGlobalObjectIdentification()

    //Db Context
    .RegisterDbContext<WalkerPlanerDbContext>()    

    //Maximum Query Depth
    .AddMaxExecutionDepthRule(5)

    //Complexity Analyzer
    .UseOperationComplexityAnalyzer()

    //Persisted Queries
    .UsePersistedQueryPipeline()
    .AddReadOnlyFileSystemQueryStorage("./queries")

    .ModifyOptions(o => {
        o.EnableDefer = true;
    })
    .ModifyRequestOptions(o =>
    {
        //Timeout
        o.ExecutionTimeout = TimeSpan.FromSeconds(360);
        
        
        //Complexity
        o.Complexity.MaximumAllowed = 10;


        o.Complexity.ApplyDefaults = true;
        o.Complexity.DefaultComplexity = 1;
        o.Complexity.DefaultResolverComplexity = 5;

        //PersistedQueries
        o.OnlyAllowPersistedQueries = false;

    });

var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();


app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    EnableSchemaRequests = false,
    AllowedGetOperations = AllowedGetOperations.All,
    Tool = {
        Enable = env.IsDevelopment()
    }
    
});

app.MapGraphQLVoyager();
app.Run();
