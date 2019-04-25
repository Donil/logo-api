using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logo.Api.ApiControllers.Dto
{
    /// <summary>
    /// Base64 encoded data DTO.
    /// </summary>
    public class Base64Dto
    {
        /// <summary>
        /// Base64 encoded data.
        /// </summary>
        /// <returns></returns>
        [Required]
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Format of encoded file.
        /// </summary>
        /// <returns></returns>
        [Required]
        [JsonProperty("format")]
        public string Format { get; set; }
    }
}
