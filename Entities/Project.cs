namespace ProjectTimer.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Clock> Clock { get; set; }
        public ICollection<SavedProjectTime> SavedProjectTime { get; set; }

        public Project() { }
        public Project(string name)
        {
            Name = name;
        }
    }
}
