using GraphQL.Common;
using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    public class RegisterDogPayload : DogPayloadBase
    {
        public RegisterDogPayload(Dog dog)
            : base(dog)
        {
        }

        public RegisterDogPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}