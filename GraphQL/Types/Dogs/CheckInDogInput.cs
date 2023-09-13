using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    public record CheckInDogInput(
        [property: ID]
        int SessionId,
        [property: ID]
        int DogId);
}