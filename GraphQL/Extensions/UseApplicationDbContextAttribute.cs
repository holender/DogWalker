using System.Reflection;
using HotChocolate.Types.Descriptors;
using Infrastructure;

namespace GraphQL.Extensions
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        protected override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<WalkerPlanerDbContext>();
        }
    }
}
