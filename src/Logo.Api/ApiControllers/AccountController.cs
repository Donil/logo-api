using System;
using System.Linq;
using System.Threading.Tasks;
using Logo.Api.ApiControllers.Dto;
using Logo.Api.Services;
using Logo.DataAccess.Managers;
using Logo.DataAccess.Models;
using Logo.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Logo.Api.ApiControllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ISmsSender smsSender;
        private readonly ILogger logger;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="smsSender"></param>
        /// <param name="loggerFactory"></param>
        public AccountController(
            UserManager userManager,
            RoleManager roleManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.smsSender = smsSender;
            this.logger = loggerFactory.CreateLogger<AccountController>();
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]RegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUser appUser = await userManager.CreateAsync(model.Email, model.FirstName, model.LastName, model.Password);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                // await signInManager.SignInAsync(appUser, isPersistent: false);
                logger.LogInformation(3, "User created a new account with password.");

                var userDto = new UserDto(appUser);
                return new ObjectResult(userDto);
            }
            catch (LogoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                throw;
            }
        }

        /// <summary>
        /// Get a current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            ApplicationUser appUser = await GetCurrentUserAsync();
            if (appUser == null)
            {
                return Unauthorized();
            }
            return new ObjectResult(new UserDto(appUser));
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
