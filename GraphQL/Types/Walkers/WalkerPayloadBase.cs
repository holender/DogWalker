using GraphQL.Common;
using Infrastructure.Data;

namespace GraphQL.Types.Walkers
{
    public class WalkerPayloadBase : Payload
    {
        protected WalkerPayloadBase(Walker walker)
        {
            Walker = walker;
        }

        protected WalkerPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Walker? Walker { get; init; }
    }
}