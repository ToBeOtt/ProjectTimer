using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.ProjectSessionTimers;
using ProjectTimer.Services.Sessions;

namespace ProjectTimer.Pages.ActiveSession
{
    public class IndexModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly SessionService _sessionService;
        private readonly ProjectSessionTimerService _projectSessionTimerService;

        public IndexModel(ProjectService projectService, SessionService sessionService, 
            ProjectSessionTimerService projectSessionTimerService)
        {
            _projectService = projectService;
            _sessionService = sessionService;
            _projectSessionTimerService = projectSessionTimerService;
        }


        [BindProperty]
        public List<Project> projectList { get; set; } = new List<Project>();
        [BindProperty]
        public Session OnGoingSession { get; set; }
        [BindProperty]
        public string TimeElapsed { get; set; }
        

        public async Task<IActionResult> OnGet(int id)
        {
            // Ta in projekt till "lägg till projekt"-dropdown
            var projects = _projectService.GetProjects();
            projectList.AddRange(projects);

            var fetchedSession = _sessionService.GetSession(id);
            OnGoingSession = fetchedSession;

            // Ta fram hur lång tid det gått av session:
            string timePassed = _sessionService.TimePassed(OnGoingSession.Started);
            TimeElapsed = timePassed;
            return Page();
        }



// TAR IN VAL AV PROJEKT OCH ORDNAR LOGIK FÖR START/STOPP AV PROJEKTTIMER.
        [BindProperty]
        public int ProjectChoice { get; set; }
        [BindProperty]
        public bool ProjectTimerStarted { get; set; }

        //public void OnPost(int projectId)
        //{
        //    ProjectTimerStarted = true;
        //    var currentProject = _projectService.GetProjectById(projectId);
        //    var currentSession = OnGoingSession;
        //    //SessionProjectTimerClock pst = new SessionProjectTimer(currentProject, currentSession, DateTime.Now);
        //    _projectSessionTimerService.CreateProjectSessionTimer(currentSession.Id, projectId);
        //}
    }
}
