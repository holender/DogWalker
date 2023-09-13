using Infrastructure.Data;

namespace GraphQL.Types.Tracks
{
    public record RenameTrackInput([property: ID] int Id, string Name);
}