using Logo.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Logo.DataAccess.Repositories
{
    public class UserStore: UserStore<ApplicationUser, ApplicationRole, AppDbContext>
    {
        public UserStore(AppDbContext context, IdentityErrorDescriber describer = null) 
            : base(context, describer)
        {
        }

        public override Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Users.Include(u => u.User).Where(u => u.Id == userId).FirstOrDefaultAsync();
        }
    }
}
