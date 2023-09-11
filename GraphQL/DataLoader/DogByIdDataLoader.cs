using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class DogByIdDataLoader : BatchDataLoader<int, Dog>
    {
        private readonly IDbContextFactory<WalkerPlanerDbContext> _dbContextFactory;

        public DogByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<WalkerPlanerDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? 
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, Dog>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = 
                _dbContextFactory.CreateDbContext();

            return await dbContext.Dogs
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}