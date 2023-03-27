using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.Sessions;
using ProjectTimer.ViewComponents;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

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

        [BindProperty]
        public List<Project> ProjectList { get; set; } = new List<Project>();
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
            var projects = _projectService.GetProjects();
            ProjectList.AddRange(projects);
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
   
        [BindProperty]
        public bool ProjectTimeractivated { get; set; }
        [BindProperty]
        public int pId { get; set; }
        [BindProperty]
        public int sId { get; set; }
        [BindProperty]
        public int ProjectName { get; set; }

        public void OnPostActivateProjectTimer(string projectName)
        {
            pId = _projectService.GetProjectByName(projectName);
            var sessionID = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
            sId = Convert.ToInt32(sessionID);
            ProjectTimeractivated = true;
        }

        public void OnPostCloseProjectTimer(string projectName)
        {
            //Avslutar projekttimer.
            // projekt-timer active == false => klocka syns inte
            // det går att välja projekt igen.
        }

    }
}
