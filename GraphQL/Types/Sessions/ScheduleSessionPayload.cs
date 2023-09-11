using GraphQL.Common;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Sessions
{
    public class ScheduleSessionPayload : SessionPayloadBase
    {
        public ScheduleSessionPayload(Session session)
            : base(session)
        {
        }

        public ScheduleSessionPayload(UserError error)
            : base(new[] { error })
        {
        }

        public async Task<Track?> GetTrackAsync(
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            if (Session is null)
            {
                return null;
            }

            return await trackById.LoadAsync(Session.Id, cancellationToken);
        }

        [UseWalkerPlanerDbContext]
        public async Task<IEnumerable<Walker>?> GetWalkersAsync(
            WalkerPlanerDbContext dbContext,
            WalkerByIdDataLoader walkerById,
            CancellationToken cancellationToken)
        {
            if (Session is null)
            {
                return null;
            }

            var walkerIds = await dbContext.Session
                .Where(s => s.Id == Session.Id)
                .Include(s => s.SessionWalkers)
                .SelectMany(s => s.SessionWalkers.Select(t => t.WalkerId))
                .ToArrayAsync(cancellationToken);

            return await walkerById.LoadAsync(walkerIds, cancellationToken);
        }
    }
}