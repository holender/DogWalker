using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public interface IDogByIdDataLoader : IDataLoader<int, Dog> { }
    public class DogByIdDataLoader : BatchDataLoader<int, Dog>, IDogByIdDataLoader
    {
        private readonly IServiceProvider _services;

        public DogByIdDataLoader(
            IBatchScheduler batchScheduler,
            IServiceProvider services)
            : base(batchScheduler)
        {
            _services = services ??
                        throw new ArgumentNullException(nameof(services));
        }

        protected override async Task<IReadOnlyDictionary<int, Dog>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var scope = _services.CreateAsyncScope();
            var p1 = scope.ServiceProvider.GetRequiredService<WalkerPlanerDbContext>();

            return await p1.Dogs
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}