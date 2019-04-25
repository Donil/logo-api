using System.Collections.Generic;
using Logo.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Logo.DataAccess.Managers
{
    /// <summary>
    /// Role manager.
    /// </summary>
    public class RoleManager: RoleManager<ApplicationRole>
    {
        public RoleManager(IRoleStore<ApplicationRole> store,
                           IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
                           ILookupNormalizer keyNormalizer,
                           IdentityErrorDescriber errors,
                           ILogger<RoleManager<ApplicationRole>> logger,
                           IHttpContextAccessor contextAccessor)
            :base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}