using Microsoft.EntityFrameworkCore;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Data;

namespace ProjectTimer.Services.ProjectTimerUsers
{
    public class ProjectTimerUserService
    {
        private readonly DataContext _context;

        public ProjectTimerUserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ProjectTimerUser> GetProjectTimerUserById(string userId)
        {
            return _context.ProjectTimerUsers.Where(p => p.Id == userId).FirstOrDefault();
        }
    }
}
