using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace WebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class ApplicationUser : IdentityUser
{
    [Required]
    public string? FullName { get; set; }
    [NotMapped]
    public string Role { get; set; }
}

