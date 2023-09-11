using GraphQL.Extensions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Query
    {
        //[UseApplicationDbContext]
        public IQueryable<Walker> GetWalkers(
            [Service] WalkerPlanerDbContext context) =>
            context.Walkers;
    }
}
