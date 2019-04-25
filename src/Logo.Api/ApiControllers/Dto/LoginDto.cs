using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Logo.Api.ApiControllers.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
