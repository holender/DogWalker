using GraphQL.Extensions;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddCors();

builder.Services.AddPooledWalkerPlanerDbContext();

builder.Services
    .AddGraphQLServer()
    .AddTypes()
    .AddUploadType()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions()
    .AddGlobalObjectIdentification()
    .AddInstrumentation(o =>
    {
        o.RenameRootActivity = true;
        o.IncludeDocument = true;
    })
    .RegisterDbContext<WalkerPlanerDbContext>()    
    .ModifyOptions(o => o.EnableDefer = true);

var app = builder.Build();

app.UseWebSockets();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapGraphQL();
app.Run();
