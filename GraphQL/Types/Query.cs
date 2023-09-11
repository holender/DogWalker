using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class Query
    {
        [UseApplicationDbContext]
        public Task<List<Walker>> GetWalkers(
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            context.Walkers.ToListAsync(cancellationToken);

        public Task<Walker> GetWalkerAsync(
            int id,
            WalkerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
