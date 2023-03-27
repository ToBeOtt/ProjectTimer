using Microsoft.AspNetCore.Mvc;
using ProjectTimer.Entities;
using ProjectTimer.Services.Projects;

namespace ProjectTimer.ViewComponents
{
    public class GetProjectSelect : ViewComponent
    {
        private readonly ProjectService _projectService;

        public GetProjectSelect(ProjectService projectService)
        {
            _projectService = projectService;
        }

        public IViewComponentResult Invoke()
        {
            List<Project> projectList = new List<Project>();
            var projects = _projectService.GetProjects();
            projectList.AddRange(projects);
            return View(projectList);   
        }

    }
}
