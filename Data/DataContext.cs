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
        public DbSet<SessionProject> SessionProject { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TimerClock> TimerClocks { get; set; }
        public DbSet<SavedProjectTime> SavedProjectTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure relations between project and sessions
            modelBuilder.Entity<SessionProject>()
                    .HasKey(sp => new { sp.ProjectId, sp.SessionId });
            modelBuilder.Entity<SessionProject>()
                    .HasOne(p => p.Project)
                    .WithMany(sp => sp.SessionProject)
                    .HasForeignKey(p => p.ProjectId);
            modelBuilder.Entity<SessionProject>()
                    .HasOne(s => s.Session)
                    .WithMany(sp => sp.SessionProject)
                    .HasForeignKey(s => s.SessionId);

        }
    }
}
