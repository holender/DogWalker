using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public interface IWalkerByIdDataLoader : IDataLoader<int, Walker> { }
    public class WalkerByIdDataLoader : BatchDataLoader<int, Walker>, IWalkerByIdDataLoader
    {
        private readonly IServiceProvider _services;

        public WalkerByIdDataLoader(
            IBatchScheduler batchScheduler,
            IServiceProvider services)
            : base(batchScheduler)
        {
            _services = services ??
                        throw new ArgumentNullException(nameof(services));
        }

        protected override async Task<IReadOnlyDictionary<int, Walker>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var scope = _services.CreateAsyncScope();
            var p1 = scope.ServiceProvider.GetRequiredService<WalkerPlanerDbContext>();

            var test =  await p1.Walkers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);

            return test;
        }
    }
}