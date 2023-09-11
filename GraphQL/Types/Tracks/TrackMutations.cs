using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Tracks
{
    [ExtendObjectType(Name = OperationTypeNames.Mutation)]
    public class TrackMutations
    {
        [UseWalkerPlanerDbContext]
        public async Task<AddTrackPayload> AddTrackAsync(
            AddTrackInput input,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken)
        {
            var track = new Track { Name = input.Name };
            context.Tracks.Add(track);

            await context.SaveChangesAsync(cancellationToken);

            return new AddTrackPayload(track);
        }

        [UseWalkerPlanerDbContext]
        public async Task<RenameTrackPayload> RenameTrackAsync(
            RenameTrackInput input,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken)
        {
            var track = await context.Tracks.FindAsync(input.Id);
            track.Name = input.Name;

            await context.SaveChangesAsync(cancellationToken);

            return new RenameTrackPayload(track);
        }
    }
}