using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace GraphQL.Configuration
{
    public class IntrospectionInterceptor: DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            if (context.Request.Headers.ContainsKey("X-Allow-Introspection"))
            {
                requestBuilder.AllowIntrospection();
            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }
    }
}
