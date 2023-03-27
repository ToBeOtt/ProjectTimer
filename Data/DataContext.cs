using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectTimer.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectTimer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Project> Projects {get; set;}
        public DbSet<ProjectSessionTimer> ProjectSessionTimers { get; set; }
        public DbSet<Session> Sessions { get; set; }
            
    }
}
