using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectTimer.Data;
using ProjectTimer.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Cryptography;
using Dapper;
using Microsoft.AspNetCore.Http;
using ProjectTimer.Services.Projects;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ProjectTimer.Areas.Identity.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectTimer.Services.Clocks
{
    public class ClockService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClockService(DataContext context, IConfiguration configuration, ProjectService projectService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Read / Sorting-services
        public async Task<List<Clock>> GetClockByDate(string userId)
        {
            var start = DateTime.Now.Date;
            var end = start.AddDays(1);

            return _context.Clocks
                .OrderBy(c => c.Started)
                .Where(c => c.Started >= start && c.Started < end)
                .Where(c => c.Project.UserId == userId)
                .ToList();
        }
        public async Task<Clock> GetClockById(int id)
        {
            return _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<Clock> GetClockByProjectId(int id)
        {
            return _context.Clocks.Where(c => c.Project.Id == id).FirstOrDefault();
        }

        public async Task<List<int>> ClockSortedByProject()
        {
            var start = DateTime.Now.Date;
            var end = start.AddDays(1);
            List<int> clocksIdList = new List<int>();  

            var parameters = new { Start = start, End = end };
            var sql = "SELECT p.Name, c.Id FROM Clocks c INNER JOIN Projects p ON c.ProjectId = p.Id where c.Started BETWEEN @Start AND @End AND c.ProjectId = p.Id GROUP BY p.Name, c.Id ORDER BY p.Name;";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@Start", start);
                command.Parameters.AddWithValue("@End", end);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int clockIds = Convert.ToInt32(reader[1]);
                        clocksIdList.Add(clockIds);
                    }
                }
                return clocksIdList;
            }
        }
       
        // Add / Update
        public async Task<Clock> CreateClock(string taskDescription, int pId, string UserId)
        {
            Clock clock = new Clock(taskDescription, DateTime.Now);

            var project = _context.Projects.Where(p => p.Id == pId).FirstOrDefault();
            var user = _context.Users.Where(pt => pt.Id == UserId).FirstOrDefault();

            clock.Project = project;
            clock.Project.User = user;

            _context.Clocks.Add(clock);
            Save();
            return clock;
        }

        public async Task<bool> UpdateClock(Clock clock)
        {
            _context.Clocks.Update(clock);
            return Save();
        }


        // Remove 
        public async Task<bool> EndClock(int id)
        {
            var clock = _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
            clock.Ended = DateTime.Now;
            clock.TotalMinutes = await TimerTimeInMinutes(clock);
            _context.Clocks.Update(clock);
            return Save();
        }

        public async Task<bool> DeleteClock(Clock clock)
        {
            _context.Clocks.Remove(clock);
            return Save();
        }

        public async Task<bool> ClockExists(int id)
        {
            return _context.Clocks.Any(p => p.Id == id);
        }

        // Other
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        [BindProperty]  // Added to get access to nested properties in Clock such as Project.
        public List<Project> ProjectList { get; set; } = new List<Project>();
        public async Task<List<List<Clock>>> CalculateSessionTime(List<int> list)
        {
            // Get current user-id
            ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var projects = await _projectService.GetProjects(Convert.ToString(currentUserID));
            ProjectList.AddRange(projects);

            var projectSeparatedList = new List<List<Clock>>();
            int counter = 0;
            projectSeparatedList.Add(new List<Clock>());
            int projectId = -1;
            foreach (var clockId in list)
            {

                while (clockId == list[0])
                {
                    Clock firstClock = await GetClockById(clockId);
                    projectId = firstClock.Project.Id;
                    break;
                }

             Clock clock = await GetClockById(clockId);

                if(clock.Project.Id == projectId)
                {
                    projectSeparatedList[counter].Add(clock);
                }

                else
                {
                    projectId = clock.Project.Id;
                    projectSeparatedList.Add(new List<Clock>());
                    counter++;
                    projectSeparatedList[counter].Add(clock);
                }                
            }
            return projectSeparatedList;
        }

        public async Task<string> DebitProjectTimeInHours(double totalTimeToCalculate)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(totalTimeToCalculate);
            string formattedTime = $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}min";
            return formattedTime;
        }


        public async Task<int> TimerTimeInMinutes(Clock clock)
        {
            DateTime Start = clock.Started;
            DateTime End = clock.Ended;

            TimeSpan result = End.Subtract(Start);
            int roundedMinutes = (int)Math.Round(result.TotalMinutes);
            return roundedMinutes;

        }
    }
}
