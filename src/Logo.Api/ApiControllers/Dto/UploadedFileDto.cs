using Newtonsoft.Json;

namespace Logo.Api.ApiControllers.Dto
{
    /// <summary>
    /// Uploaded file DTO.
    /// </summary>
    public class UploadedFileDto
    {
        /// <summary>
        /// Url.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
