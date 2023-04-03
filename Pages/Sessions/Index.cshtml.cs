using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Entities;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.ProjectTimerUsers;
using ProjectTimer.Services.Sessions;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectTimer.Pages.Sessions
{
    [BindProperties]
    [Authorize]
    public class IndexModel : PageModel
    {
        // Deklarerar ny session-key
        public const string SessionKeyId = "_Id";

        private readonly ProjectService _projectService;
        private readonly ClockService _clockService;
        private readonly ProjectTimerUserService _projectTimerUserService;

        public IndexModel(ProjectService projectService, ClockService clockService, ProjectTimerUserService projectTimerUserService)
        {
            _projectService = projectService;
            _clockService = clockService;
            _projectTimerUserService = projectTimerUserService;
        }



// OnGet views all current projects available so a new session can be started. If session is active this method
// get modelstate for current active clock. This is then displayed instead when entering the page.
        public List<Project> ProjectList { get; set; } = new List<Project>();
        public List<Clock> SummaryOfSessionClocks { get; set; } = new List<Clock>();
        public bool NoCurrentProjectBool { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //If scrolling from other pages while session is ongoing:
            if(HttpContext.Session.GetString("_Id") != null)
            {
                var stringId = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
                int Id = Convert.ToInt32(stringId);
                Clock = await _clockService.GetClockById(Id);
            }

            // Get current user-id
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(currentUser == null)
            {
                ModelState.AddModelError("", "Kan inte hitta användare");
            }

            //See if any active project exists

            var projects = await _projectService.GetProjects(currentUserID);
            if(projects == null)
            {
                return BadRequest(ModelState);
            }

            ProjectList.AddRange(projects);
            if (projects.Count <= 0)
            {
                NoCurrentProjectBool = true;
                return Page();
            }

            // Get Clocks that might exists for todays date.  // BORDE VARA GET CLOCKS BY DATE AND PROJECT ID WHERE PROJECTTIMERUSER == currentUser
            var result = await _clockService.GetClockByDate(currentUserID);
            if (result == null)
            {
                return BadRequest(ModelState);
            }
            SummaryOfSessionClocks.AddRange(result);
            return Page();
        }

        // Start new clock and begin a new session
        public Clock Clock { get; set; }
        public async Task<IActionResult> OnPostStartClock(int pID, string taskDescription)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUser == null)
            {
                ModelState.AddModelError("", "Kan inte hitta användare");
            }
            
            Clock = await _clockService.CreateClock(taskDescription, pID, currentUserID);

            if (HttpContext == null)
            {
                // HttpContext is not available
                return StatusCode(StatusCodes.Status500InternalServerError, "Något gick fel med anslutningen");
            }

            // Create a new session if session is empty, else return session ongoing.
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString
                (SessionKeyId)))
            {
                HttpContext.Session.SetInt32(SessionKeyId, Clock.Id);
            }
            else
            {
                var id = HttpContext.Session.GetInt32(SessionKeyId).ToString();
            }
            return Page();
        }

// Updates clock with end-datetime and then remove current session. 
        public async Task<IActionResult> OnPostEndClock(int clockId)
        {
            if (!await _clockService.EndClock(clockId))
            {
                ModelState.AddModelError("", "Något gick fel vid stoppandet av timern");
            }

            HttpContext.Session.Remove(SessionKeyId);
            return RedirectToPage("index");
        }

        public async Task<IActionResult> OnPostDeleteClock(int deleteId)
        {
            var result = await _clockService.GetClockById(deleteId);
            if (!await _clockService.DeleteClock(result))
            {
                ModelState.AddModelError("", "Det gick inte att radera timer");
            }
                
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
            List<int> result = await _clockService.ClockSortedByProject();
            if (!result.Any())
            {
                ModelState.AddModelError("", "Det finns inga timers att visa");
                return StatusCode(500, ModelState);

            }
            // Retrieves all clocks ordered in multiarray-list where each project is in a separate list.
            var collectedClocklists = await _clockService.CalculateSessionTime(result);
            if (!collectedClocklists.Any())
            {
                ModelState.AddModelError("", "Listan med timers gick inte att hämta..");
                return StatusCode(500, ModelState);
            }
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
}
