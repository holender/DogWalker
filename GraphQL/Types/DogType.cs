using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class DogType : ObjectType<Dog>
    {
        protected override void Configure(IObjectTypeDescriptor<Dog> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<DogByIdDataLoader>().LoadAsync(id, ctx.RequestAborted)!);

            descriptor
                .Field(t => t.SessionDogs)
                .ResolveWith<DogResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseDbContext<WalkerPlanerDbContext>()
                .Name("sessions");
        }

        protected class DogResolvers
        {
            [UseWalkerPlanerDbContext]
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Dog dog,
                WalkerPlanerDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var walkerIds = await dbContext.Dogs
                    .Where(a => a.Id == dog.Id)
                    .Include(a => a.SessionDogs)
                    .SelectMany(a => a.SessionDogs.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken);

                return await sessionById.LoadAsync(walkerIds, cancellationToken);
            }
        }
    }
}