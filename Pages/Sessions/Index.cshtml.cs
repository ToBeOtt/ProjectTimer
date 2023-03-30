using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
using System.Text;

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



// OnGet views all current projects available so a new session can be started. If session is active this method
// get modelstate for current active clock. This is then displayed instead when entering the page.
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


// Start new clock and begin a new session
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


        public bool ViewSummary { get; set; }
        public List<List<Clock>> ListOfTimerSummaries { get; set; } = new List<List<Clock>>(); 
        public List<double> TotalPricePerProjectList { get; set; } = new List<double>(); 

        // Call methods relating to summarize all clock in each project. All time that remain after truncating to nearest 30 min 
        // is then saved as new object SavedProjectTime.
        public async Task<IActionResult> OnPostSumAll()
        {
            // Gets all clocks of todays date in list grouped by project-name.
            List<int> result = _clockService.ClockSortedByProject();
            // Retrieves all clocks ordered in multiarray-list where each project is in a separate list.
            var collectedClocklists = _clockService.CalculateSessionTime(result);

            // All is presented in a ordered list where each clock is presented (time spent and when with note) and also 
            // the total time spent of session/per project.
            foreach (var clocklist in collectedClocklists)
            {
                // Values and references for current list-iteration.
                double totalTimeOfProject = 0;
                List<Clock> clockList = new List<Clock>();
                string pName = null;
                foreach (var clock in clocklist)
                {
                    pName = clock.Project.Name;
                    totalTimeOfProject = totalTimeOfProject + (double)clock.TotalMinutes;
                    clockList.Add(clock);
                }
                ListOfTimerSummaries.Add(clockList);
                TotalPricePerProjectList.Add(totalTimeOfProject);
            }
            // All info get saved in a timesheet containing Project, Date and all clock-timers of the day. This also contain deducted time
            // as SavedProjectTime.
            ViewSummary = true;
            return Page();
        }
    }

    // User is prompted with how much undebited saved time remains and asked how much should be saved.
}
