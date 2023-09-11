using Infrastructure.Data;

namespace GraphQL.Types.Sessions
{
    public record AddSessionInput(
        string Title,
        string? Description,
        [ID(nameof(Walker))]
        IReadOnlyList<int> WalkerIds);
}