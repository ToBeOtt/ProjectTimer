using ProjectTimer.Data;
using ProjectTimer.Entities;

namespace ProjectTimer.Services.Sessions
{
    public class SessionService
    {
        private readonly DataContext _context;
        public SessionService(DataContext context)
        {
            _context = context;
        }

        public bool CreateSession(Session session)
        {
            _context.Add(session);
            return Save();
        }


        public Session GetSession(int Id)
        {
            return _context.Sessions.Where(p => p.Id == Id).FirstOrDefault();
        }

        public string TimePassed(DateTime start)
        {
            DateTime TimeNow = DateTime.Now;
            TimeSpan span = TimeNow.Subtract(start);
            string timeElapsed = "Tidsåtgång: Timmar: " + span.Hours + " - min: " + span.Minutes;
            return timeElapsed;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
