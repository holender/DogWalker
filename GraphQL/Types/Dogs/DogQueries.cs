using GraphQL.DataLoader;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    [ExtendObjectType(Name = "Query")]
    public class DogQueries
    {
        [UsePaging]
        public IQueryable<Dog> GetDogs(
            WalkerPlanerDbContext context) =>
            context.Dogs;

        public Task<Dog> GetDogByIdAsync(
            [ID(nameof(Dog))] int id,
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