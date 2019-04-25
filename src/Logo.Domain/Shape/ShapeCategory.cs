namespace Logo.Domain.Shape
{
    using System.ComponentModel.DataAnnotations;

    public class ShapeCategory
    {
        [Key]
        public int Id { get; set; }

        public int ShapeId { get; set; }

        [Required]
        public Shape Shape { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}
