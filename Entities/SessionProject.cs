namespace ProjectTimer.Entities
{
    public class SessionProject
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public ICollection<TimerClock> TimerClock { get; set; }
    }
}
