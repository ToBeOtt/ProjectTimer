using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTimer.Entities;
using ProjectTimer.Services.ProjectSessionTimers;

namespace ProjectTimer.ViewComponents
{
    public class AddProjectTimer : ViewComponent
    {
        private readonly ProjectSessionTimerService _projectSessionTimerService;

        public AddProjectTimer(ProjectSessionTimerService 
            projectSessionTimerService)
        {
            _projectSessionTimerService = projectSessionTimerService;
        }

        //public IViewComponentResult Invoke(int projectId, int sessionId)
        //{
        //    SessionProjectTimerClock projectSessionTimer = new SessionProjectTimerClock();
        //    var result = _projectSessionTimerService.CreateProjectSessionTimer(projectId, sessionId);
        //    return View(result);
        //}

    }
}
