using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectTimer.Areas.Identity.Data;
using ProjectTimer.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectTimer.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {

        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<SavedProjectTime> SavedProjectTimes { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}