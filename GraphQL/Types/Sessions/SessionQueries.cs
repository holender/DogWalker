using GraphQL.DataLoader;
using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Sessions
{
    [ExtendObjectType(Name = OperationTypeNames.Query)]
    public class SessionQueries
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken) =>
            await context.Session.ToListAsync(cancellationToken);

        public Task<Session> GetSessionByIdAsync(
            [ID(nameof(Session))] int id,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            sessionById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Session>> GetSessionsByIdAsync(
            [ID(nameof(Session))] int[] ids,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken) =>
            await sessionById.LoadAsync(ids, cancellationToken);
    }
}