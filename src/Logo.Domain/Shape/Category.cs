namespace Logo.Domain.Shape
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Users;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<ShapeCategory> CategoryShapes { get; set; }
    }
}
