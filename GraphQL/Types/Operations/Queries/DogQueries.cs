using GraphQL.DataLoader;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Operations.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class DogQueries
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Dog> GetDogs(
            WalkerPlanerDbContext context) =>
            context.Dogs;
        
        [NodeResolver]
        public Task<Dog> GetDogByIdAsync(
            int id,
            DogByIdDataLoader dogById,
            CancellationToken cancellationToken) =>
            dogById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Dog>> GetDogsByIdAsync(
            [ID(nameof(Dog))] int[] ids,
            DogByIdDataLoader dogById,
            CancellationToken cancellationToken) =>
            await dogById.LoadAsync(ids, cancellationToken);
    }
}