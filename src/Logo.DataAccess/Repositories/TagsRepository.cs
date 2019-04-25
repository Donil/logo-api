namespace Logo.DataAccess.Repositories 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logo.Domain.Shape;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly AppDbContext _context;

        public CategoriesRepository(AppDbContext context)
        {
            this._context = context;
        }

        public Category Get(int id)
        {
            return _context.Categories.Find(id);
        }

        public Category Get(string name)
        {
            return _context.Categories.SingleOrDefault(t => t.Name == name);
        }

        public async Task SaveAsync(Category category)
        {
            if (category.Id == 0)
            {
                await _context.AddAsync(category);
            }
            else 
            {
                Category dbCategory = _context.Categories.Find(category.Id);
                dbCategory.Name = category.Name;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}