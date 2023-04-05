using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;
using ProjectTimer.Services.SavedProjectTimes;
using ProjectTimer.Services.Users;

namespace ProjectTimer.Pages.SavedProjectTimes
{
    public class IndexModel : PageModel
    {
    private readonly ProjectService _projectService;
    private readonly SavedProjectTimeService _savedProjectTimeService;
    private readonly UserService _userService;

    public IndexModel(ProjectService projectService, SavedProjectTimeService savedProjectTimeService
        , UserService userService)
    {
        _projectService = projectService;
        _savedProjectTimeService = savedProjectTimeService;
        _userService = userService;
    }

    [BindProperty]
    public List<Project> ProjectList { get; set; } = new List<Project>();
    [BindProperty]
    public string ShowSavedTime { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var currentUserID = await _userService.GetCurrentUserId();
        if (currentUserID == null)
        {
            ModelState.AddModelError("", "Kan inte hitta användare");
            return StatusCode(500, ModelState);
        }

        var projects = await _projectService.GetProjects(currentUserID);
        if (projects == null)
        {
            ModelState.AddModelError("", "Det finns inga projekt registrerade på användaren.");
            return StatusCode(500, ModelState);
        }

        ProjectList.AddRange(projects);
        return Page();
    }


    public async Task<IActionResult> OnPostUpdateDescription(string pName, string time, string note)
    {
        double timeToDouble = Convert.ToDouble(time);

        SavedProjectTime savedProjectTime = new SavedProjectTime();
        savedProjectTime.DateSaved = DateTime.Now;
        savedProjectTime.SavedTime = timeToDouble;
        savedProjectTime.Note = note;

        var project = await _projectService.GetProjectByName(pName);
        if (project == null)
        {
            return NotFound();
        }
        savedProjectTime.Project = project;

        if (!await _savedProjectTimeService.AddSavedProjectTime(savedProjectTime))
        {
            return NotFound();
        }

        return Redirect("/SavedProjectTimes");
    }

    [BindProperty]
    public List<SavedProjectTime> SavedTimeProjectList { get; set; } = new List<SavedProjectTime>();
    [BindProperty]
    public bool SavedTimesExists { get; set; }
    public async Task<IActionResult> OnPostProjectSavedTimes(int projectSelect)
    {
        // RETAIN MODELSTATE FOR PROJECTLIST. SHOULD BE A BETTER WAY OF NOT "DRY".
        var currentUserID = await _userService.GetCurrentUserId();
        if (currentUserID == null)
        {
            ModelState.AddModelError("", "Kan inte hitta användare");
            return StatusCode(500, ModelState);
        }

        var projects = await _projectService.GetProjects(currentUserID);
        if (projects == null)
        {
            ModelState.AddModelError("", "Det finns inga projekt registrerade på användaren.");
            return StatusCode(500, ModelState);
        }
        ProjectList.AddRange(projects);

        // METHOD TO GET TIMEPOSTS
        var results = await _savedProjectTimeService.GetSavedTimeForProject(projectSelect);
        if (results == null)
        {
            SavedTimesExists = true;
            return Page();
        }
        SavedTimeProjectList.AddRange(results);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteTimePost(int deleteTimePost)
    {
        var result = await _savedProjectTimeService.GetSavedTimeById(deleteTimePost);
        if (result == null)
        {
            ModelState.AddModelError("", "Ingen tidspost hittades.");
            return StatusCode(500, ModelState);
        }

        if (!await _savedProjectTimeService.RemoveSavedProjectTime(result))
        {
            ModelState.AddModelError("", "Något gick fel vid borttagandet av tiden.");
            return StatusCode(500, ModelState);
        }
        return Redirect("/SavedProjectTimes");
    }
}
}
