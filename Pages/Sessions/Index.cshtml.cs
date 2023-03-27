using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.Sessions;
using System.Net.Http;

namespace ProjectTimer.Pages.Sessions
{
    public class IndexModel : PageModel
    {
        // Deklarerar ny session-key
        public const string SessionKeyId = "_Id";

        private readonly SessionService _sessionService;
        private readonly ProjectService _projectService;

        public IndexModel(SessionService sessionService, ProjectService projectService)
        {
            _sessionService = sessionService;
            _projectService = projectService;
        }

        [BindProperty]
        public string sessionOnGoing { get; set; }


        public async Task<IActionResult> OnPostStartSession()
        {
            Session sessiontimer = new Session(DateTime.Now);
            _sessionService.CreateSession(sessiontimer);

            // Om ingen session pågår startas en ny
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString
                (SessionKeyId)))
            {
                HttpContext.Session.SetInt32(SessionKeyId, sessiontimer.Id);
            }
            else
            {
                var id = HttpContext.Session.GetInt32(SessionKeyId).ToString();
                sessionOnGoing = "Det finns redan en pågående session.";
            }
            return Page();
        }

        [BindProperty]
        public int viewSessionId { get; set;}
        public void OnPostViewSession()
        {
           var result = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
           viewSessionId = Convert.ToInt32(result);
        }

        public void OnPostDeleteSession()
        {
            HttpContext.Session.Remove(IndexModel.SessionKeyId);
        }


    }
}
