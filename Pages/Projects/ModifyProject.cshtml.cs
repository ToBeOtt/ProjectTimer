using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;

namespace ProjectTimer.Pages.Projects
{
    public class ModifyProjectModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly ClockService _clockService;

        public ModifyProjectModel(ProjectService projectService, ClockService clockService)
        {
            _projectService = projectService;
            _clockService = clockService;
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
            var project = await _projectService.GetProjectById(id);
            _projectService.DeleteProject(project);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Projekt kunde inte raderas.");
                return StatusCode(500, ModelState);
            }

            return Redirect("/Projects");
        }
    }
}
