using Infrastructure.Data;

namespace GraphQL.Types;

public class AddWalkerPayload
{
    public Walker Walker { get; }

    public AddWalkerPayload(Walker walker)
    {
        this.Walker = walker;
    }
}