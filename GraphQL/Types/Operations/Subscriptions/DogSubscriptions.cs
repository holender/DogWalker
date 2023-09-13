using GraphQL.DataLoader;
using GraphQL.Types.Dogs;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Infrastructure.Data;

namespace GraphQL.Types.Operations.Subscriptions
{
    [ExtendObjectType(Name = "Subscription")]
    public class DogSubscriptions
    {
        [Subscribe(With = nameof(SubscribeToOnDogCheckedInAsync))]
        public SessionDogCheckIn OnDogCheckedIn(
            [ID(nameof(Session))] int sessionId,
            [EventMessage] int dogId,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            new SessionDogCheckIn(dogId, sessionId);

        public async Task<ISourceStream<string>> SubscribeToOnDogCheckedInAsync(
            int sessionId,
            [Service] ITopicEventReceiver eventReceiver,
            CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<string>(
                "OnDogCheckedIn_" + sessionId, cancellationToken);
    }
}