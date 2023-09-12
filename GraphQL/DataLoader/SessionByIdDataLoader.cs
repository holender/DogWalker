using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public interface ISessionByIdDataLoader : IDataLoader<int, Session> { }
    public class SessionByIdDataLoader : BatchDataLoader<int, Session>, ISessionByIdDataLoader
    {
        private readonly IServiceProvider _services;

        public SessionByIdDataLoader(
            IBatchScheduler batchScheduler,
            IServiceProvider services)
            : base(batchScheduler)
        {
            _services = services ??
                        throw new ArgumentNullException(nameof(services));
        }

        protected override async Task<IReadOnlyDictionary<int, Session>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var scope = _services.CreateAsyncScope();
            var p1 = scope.ServiceProvider.GetRequiredService<WalkerPlanerDbContext>();

            var test = await p1.Session
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);

            return test;
        }
    }
}