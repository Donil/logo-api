namespace Logo.Domain.Shape
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesRepository
    {
        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Get a category by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category Get(int id);

        /// <summary>
        /// Get a category by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Category Get(string name);

        /// <summary>
        /// Save a category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task SaveAsync(Category category);

        /// <summary>
        /// Delete a category by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Category category);
    }
}
