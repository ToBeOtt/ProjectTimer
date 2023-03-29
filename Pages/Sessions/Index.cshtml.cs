using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProjectTimer.Entities;
using ProjectTimer.Services.Clocks;
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
        private readonly ProjectService _projectService;
        private readonly ClockService _clockService;

        public IndexModel(ProjectService projectService, ClockService clockService)
        {
            _projectService = projectService;
            _clockService = clockService;
        }

        [BindProperty]
        public List<Project> ProjectList { get; set; } = new List<Project>();

        public async Task<IActionResult> OnGet()
        {
            var projects = _projectService.GetProjects();
            ProjectList.AddRange(projects);
            return Page();
        }
        public Clock Clock { get; set; }
        public async Task<IActionResult> OnPostStartClock(int pID, string taskDescription)
        {
            Clock = _clockService.CreateClock(pID, taskDescription);
            return Page();
        }


        //[BindProperty]
        //public string sessionOnGoing { get; set; }

        //[BindProperty]
        //public List<Project> ProjectList { get; set; } = new List<Project>();
        //public async Task<IActionResult> OnPostStartSession()
        //{
        //    var projects = _projectService.GetProjects();
        //    ProjectList.AddRange(projects);
        //    return Page();
        //}

        //[BindProperty]
        //public int viewSessionId { get; set;}
        //public void OnPostViewSession()
        //{
        //   var result = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
        //   viewSessionId = Convert.ToInt32(result);
        //}

        //public void OnPostDeleteSession()
        //{
        //    HttpContext.Session.Remove(IndexModel.SessionKeyId);
        //}

        //[BindProperty]
        //public bool ProjectTimeractivated { get; set; }
        //[BindProperty]
        //public int pId { get; set; }
        //[BindProperty]
        //public int sId { get; set; }
        //[BindProperty]
        //public int ProjectName { get; set; }

        //public void OnPostActivateProjectTimer(string projectName)
        //{
        //    pId = _projectService.GetProjectByName(projectName);
        //    var sessionID = HttpContext.Session.GetInt32(IndexModel.SessionKeyId);
        //    sId = Convert.ToInt32(sessionID);
        //    ProjectTimeractivated = true;
        //}

        //public void OnPostCloseProjectTimer(string projectName)
        //{
        //    //Avslutar projekttimer.
        //    // projekt-timer active == false => klocka syns inte
        //    // det går att välja projekt igen.
        //}

    }
}
