using Infrastructure.Data;

namespace GraphQL.Types.Sessions
{
    public record AddSessionInput(
        string Title,
        string? Description,
        [property: ID]
        IReadOnlyList<int> WalkerIds);
}