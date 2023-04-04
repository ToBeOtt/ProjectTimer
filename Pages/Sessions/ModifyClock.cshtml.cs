using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.ObjectModelRemoting;
using NuGet.Packaging;
using ProjectTimer.Entities;
using ProjectTimer.Services.Clocks;
using ProjectTimer.Services.Projects;
using System.Security.Claims;

namespace ProjectTimer.Pages.Sessions
{
    public class ModifyClockModel : PageModel
    {
        private readonly ClockService _clockService;
        private readonly ProjectService _projectService;

        public ModifyClockModel(ClockService clockService, ProjectService projectService)
        {
            _clockService = clockService;
            _projectService = projectService;
        }

        [BindProperty]
        public bool EditLoad { get; set; }
        [BindProperty]
        public Clock Clock { get; set; }
        [BindProperty]
        public List<Project> ProjectList { get; set; } = new List<Project>();

        public async Task<IActionResult> OnGetEdit(int id)
        {
            if (!await _clockService.ClockExists(id))
            {
                ModelState.AddModelError("", "Timer existerar inte");
                return StatusCode(500, ModelState);
            }
            var clock = await _clockService.GetClockById(id);
            if (clock == null)
            {
                ModelState.AddModelError("", "Gick inte att hämta timer från databas.");
                return StatusCode(500, ModelState);
            }

            Clock = clock;
            EditLoad = true;
            return Page();
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var clock = await _clockService.GetClockById(id);
            if (clock == null)
            {
                ModelState.AddModelError("", "Gick inte att hämta timer från databas.");
                return StatusCode(500, ModelState);
            }
            if(! await _clockService.DeleteClock(clock))
            {
                ModelState.AddModelError("", "Timer kunde inte raderas.");
                return StatusCode(500, ModelState);
            }
            return Redirect("/Sessions");
        }
        public async Task<IActionResult> OnPostUpdateDescription(string taskDescription, int clockId)
        {
            var clock = await _clockService.GetClockById(clockId);
            if (clock == null)
            {
                ModelState.AddModelError("", "Gick inte att hämta timer från databas.");
                return StatusCode(500, ModelState);
            }

            clock.Description = taskDescription;
            if(!await _clockService.UpdateClock(clock))
            {
                ModelState.AddModelError("", "Timer kunde inte uppdateras.");
                return StatusCode(500, ModelState);
            }
            return Redirect("/Sessions");
        }
    }
}
