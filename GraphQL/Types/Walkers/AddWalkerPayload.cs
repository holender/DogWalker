using GraphQL.Common;
using Infrastructure.Data;

namespace GraphQL.Types.Walkers;

public class AddWalkerPayload : WalkerPayloadBase
{

    public AddWalkerPayload(Walker walker) : base(walker)
    { }

    public AddWalkerPayload(IReadOnlyList<UserError> errors)
        : base(errors)
    { }
}