namespace ProjectTimer.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public decimal TotalTime { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public ICollection<SessionProject> SessionProject { get; set; }

        public Session() { }
        public Session(DateTime started)
        {
            Started= started;
        }
    }
}
