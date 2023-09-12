using GraphQL.Common;
using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    public class DogPayloadBase : Payload
    {
        protected DogPayloadBase(Dog dog)
        {
            Dog = dog;
        }

        protected DogPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Dog? Dog { get; }
    }
}