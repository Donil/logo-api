using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Logo.Domain.Users;
using Logo.DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Logo.Domain.Exceptions;

namespace Logo.DataAccess.Managers
{
    /// <summary>
    /// User manager.
    /// </summary>
    public class UserManager: UserManager<ApplicationUser>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="store"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="userValidators"></param>
        /// <param name="passwordValidators"></param>
        /// <param name="keyNormalizer"></param>
        /// <param name="errors"></param>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        public UserManager(IUserStore<ApplicationUser> store,
                            IOptions<IdentityOptions> optionsAccessor,
                            IPasswordHasher<ApplicationUser> passwordHasher,
                            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
                            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                            ILookupNormalizer keyNormalizer,
                            IdentityErrorDescriber errors,
                            IServiceProvider services,
                            ILogger<UserManager<ApplicationUser>> logger)
            :base(store, 
                  optionsAccessor, 
                  passwordHasher, 
                  userValidators, 
                  passwordValidators, 
                  keyNormalizer, 
                  errors, 
                  services, 
                  logger)
        {
        }

        /// <summary>
        /// Create specified user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> CreateAsync(string email, string firstName, string lastName, string password, string role = null)
        {
            var appUser = new ApplicationUser();
            appUser.Email = appUser.UserName = email;
            appUser.User = new User 
            {
                FirstName = firstName,
                LastName = lastName
            };
            IdentityResult result = await base.CreateAsync(appUser, password);
            if (!result.Succeeded)
            {
                string errorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                throw new LogoException(errorMessage);
            }
            if (role != null) {
                await base.AddToRoleAsync(appUser, role);
                await base.AddClaimAsync(appUser, new Claim(ClaimTypes.Role, role));
            }
            return appUser;
        }
    }
}