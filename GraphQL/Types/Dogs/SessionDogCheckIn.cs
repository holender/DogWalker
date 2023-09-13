using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Dogs
{
    public class SessionDogCheckIn
    {
        [ID(nameof(Dog))]
        public int DogId { get; }

        [ID(nameof(Session))]
        public int SessionId { get; }

        public SessionDogCheckIn(int dogId, int sessionId)
        {
            DogId = dogId;
            SessionId = sessionId;
        }

        public async Task<int> CheckInCountAsync(
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            await context.Session
                .Where(session => session.Id == SessionId)
                .SelectMany(session => session.SessionDogs)
                .CountAsync(cancellationToken);

        public Task<Dog> GetDogAsync(
            DogByIdDataLoader dogById,
            CancellationToken cancellationToken) =>
            dogById.LoadAsync(DogId, cancellationToken);

        public Task<Session> GetSessionAsync(
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            sessionById.LoadAsync(DogId, cancellationToken);
    }
}