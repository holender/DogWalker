using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Walkers
{
    [ExtendObjectType(Name = OperationTypeNames.Mutation)]
    public class WalkerMutations
    {
        [UseWalkerPlanerDbContext]
        public async Task<AddWalkerPayload> AddWalkerAsync(
            AddWalkerInput input,
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
