using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Tracks
{
    [ExtendObjectType(Name = OperationTypeNames.Query)]
    public class TrackQueries
    {
        [UseWalkerPlanerDbContext]
        public async Task<IEnumerable<Track>> GetTracksAsync(
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            await context.Tracks.ToListAsync(cancellationToken);

        [UseWalkerPlanerDbContext]
        public Task<Track> GetTrackByNameAsync(
            string name,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            context.Tracks.FirstAsync(t => t.Name == name, cancellationToken);

        [UseWalkerPlanerDbContext]
        public async Task<IEnumerable<Track>> GetTrackByNamesAsync(
            string[] names,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync(cancellationToken);

        public Task<Track> GetTrackByIdAsync(
            [ID(nameof(Track))] int id,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken) =>
            trackById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Track>> GetTracksByIdAsync(
            [ID(nameof(Track))] int[] ids,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken) =>
            await trackById.LoadAsync(ids, cancellationToken);
    }
}