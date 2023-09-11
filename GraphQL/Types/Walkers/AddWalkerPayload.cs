using Infrastructure.Data;

namespace GraphQL.Types.Walkers;

public class AddWalkerPayload
{
    public Walker Walker { get; }

    public AddWalkerPayload(Walker walker)
    {
        Walker = walker;
    }
}