using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Walkers
{
    public class WalkerType : ObjectType<Walker>
    {
        protected override void Configure(IObjectTypeDescriptor<Walker> descriptor)
        {
            descriptor
                .Field(t => t.SessionWalkers)
                .ResolveWith<WalkerResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseDbContext<WalkerPlanerDbContext>()
                .Name("sessions");
        }
       
        protected class WalkerResolvers
        {
            [UseApplicationDbContext]
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Walker walker,
                WalkerPlanerDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var id = walker.Id;

                var speakerIds = await dbContext.Walkers
                    .Where(s => s.Id == walker.Id)
                    .Include(s => s.SessionWalkers)
                    .SelectMany(s => s.SessionWalkers.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken);

                return await sessionById.LoadAsync(speakerIds, cancellationToken);
            }
        }
    }
}