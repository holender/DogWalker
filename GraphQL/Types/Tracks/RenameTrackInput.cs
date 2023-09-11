using Infrastructure.Data;

namespace GraphQL.Types.Tracks
{
    public record RenameTrackInput([ID(nameof(Track))] int Id, string Name);
}