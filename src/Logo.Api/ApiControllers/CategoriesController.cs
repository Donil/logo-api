namespace Logo.Api.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Domain.Shape;
    using Microsoft.AspNetCore.Authorization;
    using Dto;
    using Microsoft.AspNetCore.Identity;
    using Logo.DataAccess.Models;

    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IShapesRepository shapesRepository;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="categoriesRepository"></param>
        /// <param name="shapesRepository"></param>
        /// <param name="userManager"></param>
        public CategoriesController(
            ICategoriesRepository categoriesRepository,
            IShapesRepository shapesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.categoriesRepository = categoriesRepository;
            this.shapesRepository = shapesRepository;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Category> categories = await categoriesRepository.GetAllAsync();
            IEnumerable<CategoryDto> dto = categories.Select(category => new CategoryDto(category));
            return Ok(dto);
        }

        /// <summary>
        /// Create a category.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser appUser = await GetCurrentUserAsync();
            var category = new Category
            {
                Name = categoryDto.Name,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.User
            };
            await categoriesRepository.SaveAsync(category);
            return Ok(new CategoryDto(category));
        }

        /// <summary>
        /// Get shapes of a categiry.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{categoryId:int}/shapes")]
        public async Task<IActionResult> Shapes(int categoryId) 
        {
            Category category = categoriesRepository.Get(categoryId);
            if (category == null) 
            {
                return NotFound();
            }
            IEnumerable<Shape> shapes = await shapesRepository.GetByCategoryAsync(category);
            IEnumerable<ShapeDto> shapesDto = shapes.Select(s => new ShapeDto(s));
            return Ok(shapesDto);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
