using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan Duration =>
            EndTime?.Subtract(StartTime ?? EndTime ?? DateTimeOffset.MinValue) ??
                TimeSpan.Zero;

        public int? TrackId { get; set; }

        public ICollection<SessionWalker> SessionWalkers { get; set; } =
            new List<SessionWalker>();

        public ICollection<SessionDog> SessionDogs { get; set; } =
            new List<SessionDog>();

        public Track? Track { get; set; }
    }
}