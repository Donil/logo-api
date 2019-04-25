using System;
using System.ComponentModel.DataAnnotations;
using Logo.Domain.BusinessCards;
using Newtonsoft.Json;

namespace Logo.Api.ApiControllers.Dto
{
    /// <summary>
    /// Business card DTO.
    /// </summary>
    public class BusinessCardDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BusinessCardDto()
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="card"></param>
        public BusinessCardDto(BusinessCard card)
        {
            Id = card.Id;
            Data = card.Data;
            ThumbnailUrl = card.ThumbnailUrl;
        }

        /// <summary>
        /// Id.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Data for present.
        /// </summary>
        /// <returns></returns>
        [Required]
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Thumbnail url.
        /// </summary>
        /// <returns></returns>
        [Url]
        [Required]
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }
}
