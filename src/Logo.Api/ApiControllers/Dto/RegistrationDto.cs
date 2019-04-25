using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Logo.Api.ApiControllers.Dto
{
    public class RegistrationDto
    {
        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
