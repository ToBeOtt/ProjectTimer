namespace ProjectTimer.Entities
{
    public class TimerClock
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }

        public int SessionProjectId { get; set; }
        public SessionProject SessionProject { get; set; }

    }
}
