using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using System.ComponentModel.Design;

namespace ProjectTimer.Pages.Projects
{
    public class SaveProjectTimeModel : PageModel
    {
        private readonly ProjectService _projectService;

        public SaveProjectTimeModel(ProjectService projectService)
        {
            _projectService = projectService;
        }

        public bool CreateActive { get; set; }
        public async Task<IActionResult> OnPostUpdateDescription(string pName, string time, string note)
        {
            double timeToDouble = Convert.ToDouble(time);

            SavedProjectTime savedProjectTime = new SavedProjectTime();
            savedProjectTime.DateSaved = DateTime.Now;
            savedProjectTime.SavedTime = timeToDouble;
            savedProjectTime.Note = note;

            var project = await _projectService.GetProjectByName(pName);
            if(project == null)
            {
                return NotFound();
            }
            savedProjectTime.Project = project;

            return Redirect("/Projects");
        }
    }
}
