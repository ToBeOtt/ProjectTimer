using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTimer.Services.Clocks;

namespace ProjectTimer.Pages.ActiveSessions
{
    public class IndexModel : PageModel
    {
        private readonly ClockService _clockService;

        public IndexModel(ClockService clockService)
        {
            _clockService = clockService;
        }

        public void OnGet(int Id)
        {

        }
    }
}
