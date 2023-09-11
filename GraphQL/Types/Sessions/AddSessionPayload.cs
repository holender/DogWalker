using GraphQL.Common;
using Infrastructure.Data;

namespace GraphQL.Types.Sessions
{
    public class AddSessionPayload : Payload
    {
        public AddSessionPayload(Session session)
        {
            Session = session;
        }

        public AddSessionPayload(UserError error)
            : base(new[] { error })
        {
        }

        public Session? Session { get; init; }
    }
}