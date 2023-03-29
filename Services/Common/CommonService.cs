using Microsoft.EntityFrameworkCore;
using ProjectTimer.Data;

namespace ProjectTimer.Services.Common
{
    public class CommonService
    {
        private readonly DataContext _context;

        public CommonService(DataContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
