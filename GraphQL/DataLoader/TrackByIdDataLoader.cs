using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public interface ITrackByIdDataLoader : IDataLoader<int, Track> { }
    public class TrackByIdDataLoader : BatchDataLoader<int, Track>, ITrackByIdDataLoader
    {
        private readonly IServiceProvider _services;

        public TrackByIdDataLoader(
            IBatchScheduler batchScheduler, 
            IServiceProvider services)
            : base(batchScheduler)
        {
            _services = services ??
                        throw new ArgumentNullException(nameof(services));
        }

        protected override async Task<IReadOnlyDictionary<int, Track>> LoadBatchAsync(
            IReadOnlyList<int> keys, 
            CancellationToken cancellationToken)
        {
            await using var scope = _services.CreateAsyncScope();
            var p1 = scope.ServiceProvider.GetRequiredService<WalkerPlanerDbContext>();

            return await p1.Tracks
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}