using GraphQL.Common;
using GraphQL.Extensions;
using HotChocolate.Subscriptions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Dogs
{
    [ExtendObjectType(Name = "Mutation")]
    public class DogMutations
    {
        [UseWalkerPlanerDbContext]
        public async Task<RegisterDogPayload> RegisterDogAsync(
            RegisterDogInput input,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken)
        {
            var Dog = new Dog
            {
                Breed = input.Bread,
                Name = input.Name,
                UserName = input.UserName,
            };

            context.Dogs.Add(Dog);

            await context.SaveChangesAsync(cancellationToken);

            return new RegisterDogPayload(Dog);
        }

        [UseWalkerPlanerDbContext]
        public async Task<CheckInDogPayload> CheckInDogAsync(
            CheckInDogInput input,
            WalkerPlanerDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var dog = await context.Dogs.FirstOrDefaultAsync(
                t => t.Id == input.DogId, cancellationToken);

            if (dog is null)
            {
                return new CheckInDogPayload(
                    new UserError("Dog not found.", "Dog_NOT_FOUND"));
            }

            dog.SessionDogs.Add(
                new SessionDog
                {
                    SessionId = input.SessionId
                });

            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(
                "OnDogCheckedIn_" + input.SessionId,
                input.DogId,
                cancellationToken);

            return new CheckInDogPayload(dog, input.SessionId);
        }
    }
}