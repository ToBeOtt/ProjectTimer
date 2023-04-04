using ProjectTimer.Data;
using ProjectTimer.Entities;

namespace ProjectTimer.Services.SaveProjectTimers
{
    public class SaveProjectTimerService
    {
        private readonly DataContext _context;

        public SaveProjectTimerService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSaveProjectTime(SavedProjectTime saveProjectTime)
        {

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
