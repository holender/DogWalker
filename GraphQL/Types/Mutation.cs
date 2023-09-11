using GraphQL.Extensions;
using GraphQL.Types.Walkers;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types
{
    public class Mutation
    {
        [UseApplicationDbContext]
        public async Task<AddWalkerPayload> AddWalkerAsync(
            AddWalkerPInput input, 
            WalkerPlanerDbContext context)
        {
            var walker = new Walker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.WebSite
            };

            context.Walkers.Add(walker);
            await context.SaveChangesAsync();

            return new AddWalkerPayload(walker);
        }
    }
}
