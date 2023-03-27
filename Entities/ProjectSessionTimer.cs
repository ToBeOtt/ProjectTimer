namespace ProjectTimer.Entities
{
    public class ProjectSessionTimer
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public Session Session { get; set; }
        public string? Description { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }


        public ProjectSessionTimer() { }
        public ProjectSessionTimer(Project project, Session session, DateTime started)
        {
            Project = project;
            Session = session;
            Started = started;
        }
    }
}
