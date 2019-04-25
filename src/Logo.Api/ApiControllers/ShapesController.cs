using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using Logo.Domain.Shape;
using Logo.DataAccess.Models;
using Logo.Api.ApiControllers.Dto;

namespace Logo.Api.ApiControllers
{
    [Route("api/[controller]")]
    public class ShapesController : Controller
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IShapesRepository shapesRepository;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="shapesRepository"></param>
        /// <param name="categoriesRepository"></param>
        /// <param name="userManager"></param>
        public ShapesController(
            IShapesRepository shapesRepository,
            ICategoriesRepository categoriesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.shapesRepository = shapesRepository;
            this.categoriesRepository = categoriesRepository;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all shapes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get() {
            IEnumerable<Shape> shapes = await shapesRepository.GetAllAsync();
            IEnumerable<ShapeDto> shapesDto = shapes.Select(s => new ShapeDto(s));
            return Ok(shapesDto);
        }

        /// <summary>
        /// Create a shape.
        /// </summary>
        /// <param name="shapeDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ShapeDto shapeDto) 
        {
            ApplicationUser appUser = await GetCurrentUserAsync();
            var shape = new Shape
            {
                Name = shapeDto.Name,
                Url = shapeDto.Url,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.User
            };
            // Add new categories.
            foreach (string categoryName in shapeDto.Categories.Select(t => t.Trim()))
            {
                Category category = categoriesRepository.Get(categoryName);
                if (category == null)
                {
                    category = new Category();
                    category.Name = categoryName;
                    category.CreatedAt = DateTime.Now;
                    category.CreatedBy = appUser.User;
                    await categoriesRepository.SaveAsync(category);
                }
                var shapeCategory = new ShapeCategory();
                shapeCategory.Shape = shape;
                shapeCategory.Category = category;
                shape.ShapeCategories.Add(shapeCategory);
            }

            await shapesRepository.SaveAsync(shape);
            return Ok(new ShapeDto(shape));
        }

        /// <summary>
        /// Delete a shape.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Shape shape = shapesRepository.Get(id);
            if (shape == null)
            {
                return NotFound();
            }
            await shapesRepository.DeleteAsync(shape);
            return NoContent();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
