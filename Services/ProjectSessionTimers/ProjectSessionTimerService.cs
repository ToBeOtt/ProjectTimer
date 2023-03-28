using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectTimer.Data;
using ProjectTimer.Entities;

namespace ProjectTimer.Services.ProjectSessionTimers
{
    public class ProjectSessionTimerService
    {
        private readonly DataContext _context;

        public ProjectSessionTimerService(DataContext context)
        {
            _context = context;
        }

        //public SessionProjectTimerClock CreateProjectSessionTimer(int projectId, int sessionId)
        //{
        //    var session = _context.Sessions.Where(h => h.Id == sessionId).FirstOrDefault();
        //    var project = _context.Projects.Where(h => h.Id == projectId).FirstOrDefault();

        //    SessionProjectTimerClock projectSessionTimer = new SessionProjectTimer(project, session, DateTime.Now);
        //    _context.ProjectSessionTimers.Add(projectSessionTimer);
        //    Save();
        //    return projectSessionTimer;
        //}
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
