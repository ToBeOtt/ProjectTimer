using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ProjectTimer.Entities
{
    public class SavedProjectTime
    {
        public int Id { get; set; }
        public DateTime DateSaved { get; set; }
        public Double SavedTime { get; set; }
        public string Note { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
