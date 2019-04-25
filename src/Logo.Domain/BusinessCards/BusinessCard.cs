namespace Logo.Domain.BusinessCards
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Logo.Domain.Users;

    public class BusinessCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Data { get; set; }

        [Url]
        [Required]
        public string ThumbnailUrl { get; set; }

        [Required]
        public User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
