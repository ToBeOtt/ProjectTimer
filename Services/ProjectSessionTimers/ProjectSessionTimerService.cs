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

        public ProjectSessionTimer CreateProjectSessionTimer(ProjectSessionTimer projectSessionTimer, 
            int projectId, int sessionId)
        {
            var session = _context.Sessions.Where(h => h.Id == sessionId).FirstOrDefault();
            projectSessionTimer.Session = session;
            var project = _context.Projects.Where(h => h.Id == projectId).FirstOrDefault();
            projectSessionTimer.Project = project;

            _context.ProjectSessionTimers.Add(projectSessionTimer);
            Save();
            return projectSessionTimer;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
