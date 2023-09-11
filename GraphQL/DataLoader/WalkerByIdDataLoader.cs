using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class WalkerByIdDataLoader : BatchDataLoader<int, Walker>
    {
        private readonly IDbContextFactory<WalkerPlanerDbContext> _dbContextFactory;

        public WalkerByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<WalkerPlanerDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, Walker>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext =
                _dbContextFactory.CreateDbContext();

            return await dbContext.Walkers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}