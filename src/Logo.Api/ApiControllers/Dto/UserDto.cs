using Newtonsoft.Json;
using System.Linq;
using Logo.DataAccess.Models;

namespace Logo.Api.ApiControllers.Dto
{
    public class UserDto
    {
        public UserDto()
        {

        }

        public UserDto(ApplicationUser appUser)
        {
            FirstName = appUser.User.FirstName;
            LastName = appUser.User.LastName;
            //var role = appUser.Roles.FirstOrDefault();
            //if (role != null)
            //{
            //    Role = role.RoleId;
            //}
        }
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
