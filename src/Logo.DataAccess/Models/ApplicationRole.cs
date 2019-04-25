using Microsoft.AspNetCore.Identity;
using System;

namespace Logo.DataAccess.Models
{
    /// <summary>
    /// Application role model.
    /// </summary>
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }
}
