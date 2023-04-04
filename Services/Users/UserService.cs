using Microsoft.EntityFrameworkCore;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Data;

namespace ProjectTimer.Services.Users
{
    public class UserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(string userId)
        {
            return _context.Users.Where(p => p.Id == userId).FirstOrDefault();
        }
    }
}
