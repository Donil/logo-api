namespace Logo.DataAccess.Repositories 
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Logo.Domain.Shape;
    using Microsoft.EntityFrameworkCore;

    public class ShapesRepositoy : IShapesRepository
    {
        private readonly AppDbContext _context;

        public ShapesRepositoy(AppDbContext context)
        {
            _context = context;
        }

        public Shape Get(int id)
        {
            return _context.Shapes.Find(id);
        }

        public async Task<IEnumerable<Shape>> GetAllAsync()
        {
            return await _context.Shapes.ToListAsync();
        }

        public async Task<IEnumerable<Shape>> GetByCategoryAsync(Category category)
        {
            return await _context.Shapes
                            .Where(x => x.ShapeCategories.Any(pt => pt.CategoryId == category.Id))
                            .ToListAsync();
        }

        public async Task SaveAsync(Shape shape)
        {
            if (shape.Id == 0)
            {
                foreach (ShapeCategory shapeCategory in shape.ShapeCategories)
                {
                    _context.ShapeCategory.Add(shapeCategory);
                }
                _context.Shapes.Add(shape);
            }
            else
            {
                Shape dbPrimitive = await _context.Shapes.SingleAsync(p => p.Id == shape.Id);
                dbPrimitive.Url = shape.Url;
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(Shape shape)
        {
            _context.Shapes.Remove(shape);
            await _context.SaveChangesAsync();
        }
    }
}