using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Types.Operations.Mutations;
using GraphQL.Types.Operations.Queries;
using GraphQL.Types.Operations.Subscriptions;
using HotChocolate.Execution.Configuration;

namespace GraphQL.Extensions
{
    public static class TypesRequestExecutorBuilderExtensions
    {
        public static IRequestExecutorBuilder ConfigureSchemaTypes(this IRequestExecutorBuilder builder)
        {
            RegisterGeneratedDataLoader(builder);

            builder.AddType<DogType>();
            builder.AddTypeExtension(typeof(DogQueries));
            builder.AddTypeExtension(typeof(DogMutations));
            builder.AddTypeExtension(typeof(DogSubscriptions));

            builder.AddType<WalkerType>();
            builder.AddTypeExtension(typeof(WalkerQueries));
            builder.AddTypeExtension(typeof(WalkerMutations));

            builder.AddType<SessionType>();
            builder.AddTypeExtension(typeof(SessionQueries));
            builder.AddTypeExtension(typeof(SessionMutations));
            builder.AddTypeExtension(typeof(SessionSubscriptions));

            builder.AddType<TrackType>();
            builder.AddTypeExtension(typeof(TrackQueries));
            builder.AddTypeExtension(typeof(TrackMutations));

            builder.ConfigureSchema(
                b => b.TryAddRootType(() => new ObjectType(d => d.Name(OperationTypeNames.Query)),
                    HotChocolate.Language.OperationType.Query));
            builder.ConfigureSchema(
                b => b.TryAddRootType(() => new ObjectType(d => d.Name(OperationTypeNames.Mutation)),
                    HotChocolate.Language.OperationType.Mutation));
            builder.ConfigureSchema(
                b => b.TryAddRootType(() => new ObjectType(d => d.Name(OperationTypeNames.Subscription)),
                    HotChocolate.Language.OperationType.Subscription));

            return builder;
        }

        public static void RegisterGeneratedDataLoader(IRequestExecutorBuilder builder)
        {
            builder.AddDataLoader<IWalkerByIdDataLoader, WalkerByIdDataLoader>();
            builder.AddDataLoader<ISessionByIdDataLoader, SessionByIdDataLoader>();
            builder.AddDataLoader<IDogByIdDataLoader, DogByIdDataLoader>();
            builder.AddDataLoader<ITrackByIdDataLoader, TrackByIdDataLoader>();
        }
    }
}
