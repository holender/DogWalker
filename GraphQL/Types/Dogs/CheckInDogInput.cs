using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    public record CheckInDogInput(
        [ID(nameof(Session))]
        int SessionId,
        [ID(nameof(Dog))]
        int DogId);
}