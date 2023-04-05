using ProjectTimer.Data;
using ProjectTimer.Entities;

namespace ProjectTimer.Services.SavedProjectTimes
{
    public class SavedProjectTimeService
    {
        private readonly DataContext _context;

        public SavedProjectTimeService(DataContext context)
        {
            _context = context;
        }
        public async Task<ICollection<SavedProjectTime>> GetSavedTimeForProject(int projectId)
        {
            return _context.SavedProjectTimes.Where(spt => spt.ProjectId == projectId).OrderBy(spt => spt.Id).ToList();
        }

        public async Task<SavedProjectTime> GetSavedTimeById(int timeId)
        {
            return _context.SavedProjectTimes.Where(spt => spt.Id == timeId).FirstOrDefault();
        }

        public async Task<bool> AddSavedProjectTime(SavedProjectTime saveProjectTime)
        {
            _context.SavedProjectTimes.Add(saveProjectTime);
            return Save();
        }

        public async Task<bool> RemoveSavedProjectTime(SavedProjectTime savedProjectTime)
        {
            _context.SavedProjectTimes.Remove(savedProjectTime);
            return Save();
        }


        // Other
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
