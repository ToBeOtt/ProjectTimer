namespace ProjectTimer.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SavedTime { get; set; }
        public ICollection<ProjectSessionTimer> ProjectSessionTimers { get; set; }

        public Project() { }
        public Project(string name)
        {
            Name = name;
        }
    }
}
