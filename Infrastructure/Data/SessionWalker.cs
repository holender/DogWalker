namespace Infrastructure.Data
{
    public class SessionWalker
    {
        public int SessionId { get; set; }

        public Session? Session { get; set; }

        public int SpeakerId { get; set; }

        public Walker? Speaker { get; set; }
    }
}