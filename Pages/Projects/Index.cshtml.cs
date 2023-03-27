using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;

namespace ProjectTimer.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly ProjectService _projectService;

        public IndexModel(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [BindProperty]
        public List<Project> projectList { get; set; } = new List<Project>();
        public void OnGet()
        {
            var projects = _projectService.GetProjects();
            projectList.AddRange(projects);
        }


        [BindProperty]
        public int Name { get; set; }
        public async Task<IActionResult> OnPost(string name)
        {
            if (name == null)
            {
                return StatusCode(500, ModelState);
            }
            Project project = new Project(name);
            if (!_projectService.CreateProject(project))
            {
                ModelState.AddModelError("", "Projekt kunde inte sparas");
                return StatusCode(500, ModelState);
            }
            return Page();
        }
    }
}
