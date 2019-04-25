using Newtonsoft.Json;

namespace Logo.Api.ApiControllers.Dto
{
    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
