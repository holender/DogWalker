namespace Infrastructure.Data
{
    public class SessionWalker
    {
        public int SessionId { get; set; }

        public Session? Session { get; set; }

        public int WalkerId { get; set; }

        public Walker? Walker { get; set; }
    }
}