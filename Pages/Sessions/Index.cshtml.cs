using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProjectTimer.Entities;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.Sessions;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ProjectTimer.Pages.Sessions
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        // Deklarerar ny session-key
        public const string SessionKeyId = "_Id";

        private readonly ProjectService _projectService;
        private readonly ClockService _clockService;

        public IndexModel(ProjectService projectService, ClockService clockService)
        {
            _projectService = projectService;
            _clockService = clockService;
        }

        public List<Project> ProjectList { get; set; } = new List<Project>();
        public List<Clock> SummaryOfSessionClocks { get; set; } = new List<Clock>();
        public async Task<IActionResult> OnGetAsync()
        {
            //If scrolling from other pages while session is ongoing:
            if(HttpContext.Session.GetString("_Id") != null)
            {
                var stringId = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
                int Id = Convert.ToInt32(stringId);
                Clock = _clockService.GetClockById(Id);
            }

            var result = _clockService.GetClockByDate();
            SummaryOfSessionClocks.AddRange(result);

            // then..
            var projects = _projectService.GetProjects();
            ProjectList.AddRange(projects);
            return Page();
        }

        public Clock Clock { get; set; }
        public string projectClockActive { get; set; }
        public async Task<IActionResult> OnPostStartClock(int pID, string taskDescription)
        {
            Clock = _clockService.CreateClock(pID, taskDescription);
            // Create a new session if session is empty, else return session ongoing.
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString
                (SessionKeyId)))
            {
                HttpContext.Session.SetInt32(SessionKeyId, Clock.Id);
            }
            else
            {
                var id = HttpContext.Session.GetInt32(SessionKeyId).ToString();
                projectClockActive = "Det finns redan en pågående session.";
                // Add later: would you like to end current session?
            }
            return Page();
        }

        // Updates clock with end-datetime and then remove current session. 
        public async Task<IActionResult> OnPostEndClock(int clockId)
        {
            _clockService.EndClock(clockId);
            HttpContext.Session.Remove(SessionKeyId);
            return RedirectToPage("index");
        }

        public async Task<IActionResult> OnPostDeleteClock(int deleteId)
        {
            var result = _clockService.GetClockById(deleteId);
            _clockService.DeleteClock(result);
            return RedirectToPage("index");
        }
     


    }
}
