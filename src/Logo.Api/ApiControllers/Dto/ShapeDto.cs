namespace Logo.Api.ApiControllers.Dto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using Domain.Shape;

    public class ShapeDto
    {
        public ShapeDto()
        {

        }

        public ShapeDto(Shape shape) 
        {
            Id = shape.Id;
            Name = shape.Name;
            Url = shape.Url;
        }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [Url]
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("categories")]
        public ICollection<string> Categories { get; set; } = new List<string>();
    }
}
