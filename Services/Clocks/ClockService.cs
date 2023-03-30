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

namespace ProjectTimer.Services.Clocks
{
    public class ClockService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;

        public ClockService(DataContext context, IConfiguration configuration, ProjectService projectService)
        {
            _context = context;
            _configuration = configuration;
            _projectService = projectService;
        }


        // Read / Sorting-services
        public List<Clock> GetClockByDate()
        {
            var start = DateTime.Now.Date;
            var end = start.AddDays(1);

            return _context.Clocks.OrderBy(c => c.Started).Where(c => c.Started >= start && c.Started < end).ToList();
        }
        public Clock GetClockById(int id)
        {
            return _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
        }

        public Clock GetClockByProjectId(int id)
        {
            return _context.Clocks.Where(c => c.Project.Id == id).FirstOrDefault();
        }

        public List<int> ClockSortedByProject()
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
        public Clock CreateClock(int projectId, string taskDescription)
        {
            Clock clock = new Clock(taskDescription, DateTime.Now);
            var project = _context.Projects.Where(h => h.Id == projectId).FirstOrDefault();
            clock.Project = project;

            _context.Clocks.Add(clock);
            Save();
            return clock;
        }
       
        
        // Remove 
        public bool EndClock(int id)
        {
            var clock = _context.Clocks.Where(c => c.Id == id).FirstOrDefault();
            clock.Ended = DateTime.Now;
            clock.TotalMinutes = TimerTimeInMinutes(clock);
            _context.Clocks.Update(clock);
            return Save();
        }

        public bool DeleteClock(Clock clock)
        {
            _context.Clocks.Remove(clock);
            return Save();
        }
        
        
        // Other
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        [BindProperty]  // Added to get access to nested properties in Clock such as Project.
        public List<Project> ProjectList { get; set; } = new List<Project>();
        public List<List<Clock>> CalculateSessionTime(List<int> list)
        {
            var projects = _projectService.GetProjects();
            ProjectList.AddRange(projects);

            var projectSeparatedList = new List<List<Clock>>();
            int counter = 0;
            projectSeparatedList.Add(new List<Clock>());
            int projectId = -1;
            foreach (var clockId in list)
            {

                while (clockId == list[0])
                {
                    Clock firstClock = GetClockById(clockId);
                    projectId = firstClock.Project.Id;
                    break;
                }

             Clock clock = GetClockById(clockId);

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

        public double DebitProjectTimeInHours(double totalTimeToCalculate)
        {
            return totalTimeToCalculate / 60;

        }


        public double TimerTimeInMinutes(Clock clock)
        {
            DateTime Start = clock.Started;
            DateTime End = clock.Ended;

            TimeSpan result = End.Subtract(Start);
            return result.TotalMinutes;

        }
    }
}
