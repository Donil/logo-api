using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Logo.Api.ApiControllers.Dto;
using Logo.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Logo.Api.ApiControllers
{
    /// <summary>
    /// Authentication controller.
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly SigningCredentials signingCredentials;
        private readonly string issuer;
        private readonly string audience;

        const string TOKENS_CONFIG_SECTION_NAME = "Tokens";
        const string KEY_CONFIG_NAME = "Key";
        const string ISSUER_CONFIG_NAME = "Issuer";
        const string AUDIENCE_CONFIG_NAME = "Audience";


        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="userManager">User manager service.</param>
        /// <param name="signInManager">Signin manager service.</param>
        /// <param name="configuration">Configuration provider.</param>
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            // Prepare parameters for JWT token.
            IConfigurationSection tokensConfig = configuration.GetSection(TOKENS_CONFIG_SECTION_NAME);
            string secretKey = tokensConfig[KEY_CONFIG_NAME];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            this.signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            this.issuer = tokensConfig[ISSUER_CONFIG_NAME];
            this.audience = tokensConfig[AUDIENCE_CONFIG_NAME];
        }

        /// <summary>
        /// Generate JWT token.
        /// </summary>
        /// <param name="model">Login DTO model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Token([FromBody]LoginDto model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

            var now = DateTime.UtcNow;
            var expiration = TimeSpan.FromDays(7);

            // Prepare claims.
            IList<Claim> userClaims = await userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64));

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: userClaims,
                notBefore: now,
                expires: now.Add(expiration),
                signingCredentials: signingCredentials);
            
            var tokenDto = new TokenDto();
            tokenDto.Token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new OkObjectResult(tokenDto);
        }
        
        /// <summary>
        /// Get this datetime as a Unix epoch timestamp (seconds since Jan 1, 1970, midnight UTC).
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>Seconds since Unix epoch.</returns>
        public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}
