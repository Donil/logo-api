namespace Logo.DataAccess.Models
{
    using Microsoft.AspNetCore.Identity;

    using Domain.Users;
    using System.ComponentModel.DataAnnotations;
    using System;

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public virtual User User { get; set; }
    }
}
