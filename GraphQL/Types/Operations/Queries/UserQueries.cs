using System.Security.Claims;
using HotChocolate.Authorization;

namespace GraphQL.Types.Operations.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class UserQueries
{   
    [AllowAnonymous]
    public string? GetMyNameIdentifier(
        [GlobalState] string? username,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        return username is null ? null : claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
