using GraphQL.DataLoader;
using HotChocolate.Authorization;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Operations.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    //[Authorize(ApplyPolicy.Validation)]
    public class WalkerQueries
    {

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Walker> GetWalkers(
            WalkerPlanerDbContext context) =>
            context.Walkers.OrderBy(t => t.Name);


       // [NodeResolver]
        public Task<Walker> GetWalkerByIdAsync(
            [ID(nameof(Walker))] int id,
            WalkerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Walker>> GetWalkersByIdAsync(
            [ID(nameof(Walker))] int[] ids,
            WalkerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(ids, cancellationToken);
    }
}
