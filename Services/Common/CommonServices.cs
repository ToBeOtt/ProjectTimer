using Microsoft.EntityFrameworkCore;
using ProjectTimer.Data;

namespace ProjectTimer.Services.Common
{
    public class CommonServices
    {
        private readonly DataContext _context;

        public CommonServices(DataContext context)
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
