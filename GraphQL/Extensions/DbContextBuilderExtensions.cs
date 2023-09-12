using HotChocolate.Execution.Configuration;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Extensions
{
    public static class DbContextExecutorBuilderExtensions
    {
        public static IServiceCollection AddPooledWalkerPlanerDbContext(this IServiceCollection service)
        {
            service.AddPooledDbContextFactory<WalkerPlanerDbContext>
                (op => op.UseSqlite("Data Source=walkerPlaner.db"));
            service.AddDbContextPool<WalkerPlanerDbContext>
                (op => op.UseSqlite("Data Source=walkerPlaner.db"));

            return service;
        }
    }
}
