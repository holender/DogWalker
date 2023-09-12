using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class TrackType : ObjectType<Track>
    {
        protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
        {
            descriptor.Field(s => s.Name).UseUpperCase();

            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<TrackByIdDataLoader>().LoadAsync(id, ctx.RequestAborted)!);

            descriptor
                .Field(t => t.Sessions)
                .ResolveWith<TrackResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseDbContext<WalkerPlanerDbContext>()
                .UsePaging<NonNullType<SessionType>>()
                .Name("sessions");
        }


        protected class TrackResolvers
        {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Track track,
                WalkerPlanerDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var sessionIds = await dbContext.Session
                    .Where(s => s.Id == track.Id)
                    .Select(s => s.Id)
                    .ToArrayAsync(cancellationToken);

                return await sessionById.LoadAsync(sessionIds, cancellationToken);
            }
        }
    }
}