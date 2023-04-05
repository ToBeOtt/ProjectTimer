using Microsoft.EntityFrameworkCore;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Data;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.Users;
using System.Security.Claims;

namespace ProjectTimer.Services.Users
{
    public class UserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetCurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<User> GetUserById(string userId)
        {
            return _context.Users.Where(p => p.Id == userId).FirstOrDefault();
        }
    }
}



