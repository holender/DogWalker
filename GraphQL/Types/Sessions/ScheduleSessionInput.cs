using Infrastructure.Data;

namespace GraphQL.Types.Sessions
{
    public record ScheduleSessionInput(
        [property: ID]
        int SessionId,
        [property: ID]
        int TrackId,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}