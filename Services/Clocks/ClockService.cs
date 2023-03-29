using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectTimer.Data;
using ProjectTimer.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace ProjectTimer.Services.Clocks
{
    public class ClockService
    {
        private readonly DataContext _context;

        public ClockService(DataContext context)
        {
            _context = context;
        }

        public Clock GetClockById(int id)
        {
            return _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
        }

        public Clock GetClockByProjectId(int id)
        {
            return _context.Clocks.Where(c => c.Project.Id == id).FirstOrDefault();
        }

        public ICollection<Clock> GetClockByDate()
        {
            var start = DateTime.Now.Date;
            var end = start.AddDays(1);

            return _context.Clocks.OrderBy(c => c.Started).Where(c => c.Started >= start && c.Started < end).ToList();
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

        public bool EndClock(int id)
        {
            var clock = _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
            clock.Ended = DateTime.Now;
            _context.Clocks.Update(clock);
            return Save();
        }

        public bool DeleteClock(Clock clock)
        {
            _context.Clocks.Remove(clock);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
