using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logo.Api.ApiControllers.Dto
{
    /// <summary>
    /// Raw SVG DTO.
    /// </summary>
    public class SvgRawDto
    {
        /// <summary>
        /// String data of SVG.
        /// </summary>
        /// <returns></returns>
        [Required]
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
