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
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public IndexModel(ProjectService projectService, ClockService clockService, UserManager<User> userManager)
        {
            _projectService = projectService;
            _clockService = clockService;
            _userManager = userManager;
        }

        [BindProperty]
        public List<Project> projectList { get; set; } = new List<Project>();
        public async Task<IActionResult> OnGetAsync()
        {
            // Get current user-id
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(currentUserID == null)
            {
                ModelState.AddModelError("", "Kan inte hitta anv�ndare");
                return StatusCode(500, ModelState);
            }

            var projects = await _projectService.GetProjects(currentUserID);
            projectList.AddRange(projects);
            return Page();
        }


        [BindProperty]
        public string Name { get; set; }
        public async Task<IActionResult> OnPostAsync(string name)
        {
            // Get current user-id
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (name == null)
            {
                return StatusCode(500, ModelState);
            }
            Project project = new Project(name, currentUserID);
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Projektet kunde inte skapas");
                return StatusCode(500, ModelState);
            }
            if (!await _projectService.CreateProject(project))
            {
                ModelState.AddModelError("", "Projekt kunde inte sparas");
                return StatusCode(500, ModelState);
            }
            Name = project.Name;
            return Redirect("/Projects");
        }
    }
}
