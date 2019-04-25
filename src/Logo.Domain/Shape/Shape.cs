namespace Logo.Domain.Shape
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Users;

    public class Shape
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        public User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<ShapeCategory> ShapeCategories { get; set; } = new List<ShapeCategory>();
    }
}
