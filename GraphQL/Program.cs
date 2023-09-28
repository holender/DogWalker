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
builder.Services.AddAuthorization(c =>
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
});

builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
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
    .ModifyOptions(o => o.EnableDefer = true);

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
