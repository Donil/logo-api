namespace Logo.Domain.Shape
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IShapesRepository
    {
        Shape Get(int id);

        Task SaveAsync(Shape shape);

        Task<IEnumerable<Shape>> GetAllAsync();

        Task<IEnumerable<Shape>> GetByCategoryAsync(Category category);

        Task DeleteAsync(Shape shape);
    }
}