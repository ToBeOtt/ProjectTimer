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
       public async Task<bool> CreateProject(Project project)
        {
            _context.Add(project);
            return Save();
        }

        public async Task<Project> GetProjectById(int id)
        {
            return _context.Projects.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task<Project> GetProjectByUser(string id)
        {
            return _context.Projects.Where(p => p.ProjectTimeUserId == id).FirstOrDefault();
        }

        public async Task<int> GetProjectByName(string projectName)
        {
            var result = _context.Projects.Where(p => p.Name == projectName).FirstOrDefault();
            int projectId = result.Id;
            return projectId;
        }

        public async Task<ICollection<Project>> GetProjects(string id)
        {
            return _context.Projects.Where(p => p.ProjectTimeUserId == id).OrderBy(p => p.Id).ToList();
        }

        public async Task<bool> DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<bool> ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }

    }
}
