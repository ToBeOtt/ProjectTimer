using Microsoft.EntityFrameworkCore;
using ProjectTimer.Data;
using ProjectTimer.Entities;
using ProjectTimer.Services.Common;

namespace ProjectTimer.Services.Projects
{
    public class ProjectService
    {
        private readonly DataContext _context;

        public ProjectService(DataContext context)
        {
            _context = context;
        }
       public bool CreateProject(Project project)
        {
            _context.Add(project);
            return Save();
        }

        public Project GetProjectById(int id)
        {
            return _context.Projects.Where(p => p.Id == id).FirstOrDefault();
        }

        public int GetProjectByName(string projectName)
        {
            var result = _context.Projects.Where(p => p.Name == projectName).FirstOrDefault();
            int projectId = result.Id;
            return projectId;
        }

        public ICollection<Project> GetProjects()
        {
            return _context.Projects.OrderBy(h => h.Id).ToList();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
