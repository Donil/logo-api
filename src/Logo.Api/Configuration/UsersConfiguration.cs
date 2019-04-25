using System.Collections.Generic;
using Logo.DataAccess.Models;

namespace Logo.Api.Configuration
{
    public static class UsersConfiguration
    {
        public static IEnumerable<string> GetRoles()
        {
            yield return "Administrator";
        }

        public static IEnumerable<CreationUser> GetUsers()
        {
            yield return new CreationUser 
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                Role = "Administrator",
                Password = "Dar00kat$"
            };
        }
    }

    /// <summary>
    /// Created user for initial configuration.
    /// </summary>
    public class CreationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}