using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectTimer.Entities;

namespace ProjectTimer.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ProjectTimerUser class
public class User : IdentityUser
{
    public ICollection<Project> Project { get; set; }
}

