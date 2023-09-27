using GraphQL.DataLoader;
using GraphQL.Types;
using HotChocolate.Authorization;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Operations.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class SessionQueries
    {

        [UsePaging(typeof(NonNullType<SessionType>))]
        [UseFiltering(typeof(SessionFilterInputType))]
        [UseSorting]
        public IQueryable<Session> GetSessions(
             WalkerPlanerDbContext context) =>
            context.Session;

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