using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Walkers
{
    [ExtendObjectType(Name = OperationTypeNames.Query)]
    public class WalkerQueries
    {

        [UsePaging]
        public IQueryable<Walker> GetWalkers(
            WalkerPlanerDbContext context) =>
            context.Walkers.OrderBy(t => t.Name);

        //[UseWalkerPlanerDbContext]
        //public Task<List<Walker>> GetWalkers(
        //    WalkerPlanerDbContext context,
        //    CancellationToken cancellationToken) =>
        //    context.Walkers.ToListAsync(cancellationToken);

        public Task<Walker> GetWalkerByIdAsync(
            [ID(nameof(Walker))]int id,
            WalkerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Walker>> GetWalkersByIdAsync(
            [ID(nameof(Walker))]int[] ids,
            WalkerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(ids, cancellationToken);
    }
}
