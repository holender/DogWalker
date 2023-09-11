using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Query
    {
        [UseApplicationDbContext]
        public Task<List<Walker>> GetWalkers(
            WalkerPlanerDbContext context) =>
            context.Walkers.ToListAsync();
    }
}
