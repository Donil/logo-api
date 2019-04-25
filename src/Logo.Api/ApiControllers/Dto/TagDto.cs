using Logo.Domain.Shape;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Logo.Api.ApiControllers.Dto
{
    public class CategoryDto
    {
        public CategoryDto()
        {

        }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
