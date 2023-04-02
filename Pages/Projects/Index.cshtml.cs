using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Entities;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;
using System.Security.Claims;

namespace ProjectTimer.Pages.Projects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly ClockService _clockService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ProjectTimerUser> _userManager;

        public IndexModel(ProjectService projectService, ClockService clockService, UserManager<ProjectTimerUser> userManager)
        {
            _projectService = projectService;
            _clockService = clockService;
            _userManager = userManager;
        }

        [BindProperty]
        public List<Project> projectList { get; set; } = new List<Project>();
        public void OnGet()
        {
            // Get current user-id
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var projects = _projectService.GetProjects(currentUserID);
            projectList.AddRange(projects);
        }


        [BindProperty]
        public int Name { get; set; }
        public async Task<IActionResult> OnPost(string name)
        {
            // Get current user-id
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (name == null)
            {
                return StatusCode(500, ModelState);
            }
            Project project = new Project(name, currentUserID);
            if (!_projectService.CreateProject(project))
            {
                ModelState.AddModelError("", "Projekt kunde inte sparas");
                return StatusCode(500, ModelState);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProject(int projectChoice)
        {
            //if-validera osv..
            var clock = _clockService.GetClockByProjectId(projectChoice);
            _clockService.DeleteClock(clock);

            var project = _projectService.GetProjectById(projectChoice);
            _projectService.DeleteProject(project);

            return Page();
        }
    }
}
