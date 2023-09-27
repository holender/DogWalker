using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace GraphQL.Configuration;

public sealed class HttpRequestInterceptor : DefaultHttpRequestInterceptor
{

    public HttpRequestInterceptor()
    {

    }

    public override async ValueTask OnCreateAsync(
        HttpContext context,
        IRequestExecutor requestExecutor,
        IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {

        if (context.Request.Headers.ContainsKey("X-Allow-Introspection"))
        {
            requestBuilder.AllowIntrospection();
        }

        
        requestBuilder.SetGlobalState("username", null);

        if (context.Request.Headers.TryGetValue("Authorization", out var value) &&
            AuthenticationHeaderValue.Parse(value) is { Parameter: { } parameters })
        {
            var user = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            requestBuilder.SetGlobalState("username", user);
        }

        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}
