using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class SessionType : ObjectType<Session>
    {
        protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<SessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted)!);

            descriptor
                .Field(t => t.SessionWalkers)
                .ResolveWith<SessionResolvers>(t => t.GetWalkersAsync(default!, default!, default!, default))
                .UseDbContext<WalkerPlanerDbContext>()
                .Name("walkers");

            descriptor
                .Field(t => t.SessionDogs)
                .ResolveWith<SessionResolvers>(t => t.GetDogsAsync(default!, default!, default!, default))
                .UseDbContext<WalkerPlanerDbContext>()
                .Name("dogs");

            descriptor
                .Field(t => t.Track)
                .ResolveWith<SessionResolvers>(t => t.GetTrackAsync(default!, default!, default));

            descriptor
                .Field(t => t.TrackId)
                .ID(nameof(Track));
        }

        protected class SessionResolvers
        {
            [UseWalkerPlanerDbContext]
            public async Task<IEnumerable<Walker>> GetWalkersAsync(
                [Parent] Session session,
                WalkerPlanerDbContext dbContext,
                WalkerByIdDataLoader WalkerById,
                CancellationToken cancellationToken)
            {
                var WalkerIds = await dbContext.Session
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionWalkers)
                    .SelectMany(s => s.SessionWalkers.Select(t => t.WalkerId))
                    .ToArrayAsync(cancellationToken);

                return await WalkerById.LoadAsync(WalkerIds, cancellationToken);
            }

            [UseWalkerPlanerDbContext]
            public async Task<IEnumerable<Dog>> GetDogsAsync(
                [Parent] Session session,
                WalkerPlanerDbContext dbContext,
                DogByIdDataLoader dogById,
                CancellationToken cancellationToken)
            {
                var attendeeIds = await dbContext.Session
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionDogs)
                    .SelectMany(s => s.SessionDogs.Select(t => t.DogId))
                    .ToArrayAsync(cancellationToken);

                return await dogById.LoadAsync(attendeeIds, cancellationToken);
            }

            public async Task<Track?> GetTrackAsync(
                Session session,
                TrackByIdDataLoader trackById,
                CancellationToken cancellationToken)
            {
                if (session.TrackId is null)
                {
                    return null;
                }

                return await trackById.LoadAsync(session.TrackId.Value, cancellationToken);
            }
        }
    }
}