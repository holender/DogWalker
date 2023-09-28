using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate.Authorization;
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
                .Name("Session")
                .Authorize(ApplyPolicy.Validation);

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
            public async Task<IEnumerable<Walker>> GetWalkersAsync(
                [Parent] Session session,
                WalkerPlanerDbContext dbContext,
                WalkerByIdDataLoader walkerById,
                CancellationToken cancellationToken)
            {
                var walkerIds = await dbContext.Session
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionWalkers)
                    .SelectMany(s => s.SessionWalkers.Select(t => t.WalkerId))
                    .ToArrayAsync(cancellationToken);

                return await walkerById.LoadAsync(walkerIds, cancellationToken);
            }

            [UseWalkerPlanerDbContext]
            public async Task<IEnumerable<Dog>> GetDogsAsync(
                [Parent] Session session,
                WalkerPlanerDbContext dbContext,
                DogByIdDataLoader dogById,
                CancellationToken cancellationToken)
            {
                var dogIds = await dbContext.Session
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionDogs)
                    .SelectMany(s => s.SessionDogs.Select(t => t.DogId))
                    .ToArrayAsync(cancellationToken);

                return await dogById.LoadAsync(dogIds, cancellationToken);
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