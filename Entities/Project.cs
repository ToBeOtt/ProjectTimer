using ProjectTimer.Areas.Identity.Data;

namespace ProjectTimer.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Clock> Clock { get; set; }
        public ICollection<SavedProjectTime>? SavedProjectTime { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }



        public Project() { }
        public Project(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
    }
}
