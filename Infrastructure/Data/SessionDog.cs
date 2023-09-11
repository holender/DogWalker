namespace Infrastructure.Data
{
    public class SessionDog
    {
        public int SessionId { get; set; }

        public Session? Session { get; set; }

        public int AttendeeId { get; set; }

        public Dog? Attendee { get; set; }
    }
}