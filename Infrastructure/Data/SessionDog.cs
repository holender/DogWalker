namespace Infrastructure.Data
{
    public class SessionDog
    {
        public int SessionId { get; set; }

        public Session? Session { get; set; }

        public int DogId { get; set; }

        public Dog? Dog { get; set; }
    }
}