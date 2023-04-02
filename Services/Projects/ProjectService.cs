﻿using Microsoft.EntityFrameworkCore;
using ProjectTimer.Data;
using ProjectTimer.Entities;
using ProjectTimer.Services.Common;

namespace ProjectTimer.Services.Projects
{
    public class ProjectService
    {
        private readonly DataContext _context;

        public ProjectService(DataContext context)
        {
            _context = context;
        }
       public bool CreateProject(Project project)
        {
            _context.Add(project);
            return Save();
        }

        public Project GetProjectById(int id)
        {
            return _context.Projects.Where(p => p.Id == id).FirstOrDefault();
        }

        public int GetProjectByName(string projectName)
        {
            var result = _context.Projects.Where(p => p.Name == projectName).FirstOrDefault();
            int projectId = result.Id;
            return projectId;
        }

        public ICollection<Project> GetProjects(string id)
        {
            return _context.Projects.Where(p => p.ProjectTimeUserId == id).OrderBy(p => p.Id).ToList();
        }

        public bool DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
