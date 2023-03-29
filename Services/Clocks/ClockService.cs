using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectTimer.Data;
using ProjectTimer.Entities;

namespace ProjectTimer.Services.Clocks
{
    public class ClockService
    {
        private readonly DataContext _context;

        public ClockService(DataContext context)
        {
            _context = context;
        }

        public Clock CreateClock(int projectId, string taskDescription)
        {
            Clock clock = new Clock(taskDescription, DateTime.Now);
            var project = _context.Projects.Where(h => h.Id == projectId).FirstOrDefault();
            clock.Project = project;

            _context.Clocks.Add(clock);
            Save();
            return clock;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
