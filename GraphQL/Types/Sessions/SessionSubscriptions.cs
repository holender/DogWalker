using GraphQL.DataLoader;
using Infrastructure.Data;

namespace GraphQL.Types.Sessions
{
    [ExtendObjectType(Name = OperationTypeNames.Subscription)]
    public class SessionSubscriptions
    {
        [Subscribe]
        [Topic]
        public Task<Session> OnSessionScheduledAsync(
            [EventMessage] int sessionId,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            sessionById.LoadAsync(sessionId, cancellationToken);
    }
}