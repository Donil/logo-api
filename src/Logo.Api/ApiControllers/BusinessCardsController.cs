using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Logo.Domain.BusinessCards;
using Microsoft.AspNetCore.Authorization;
using Logo.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Logo.Api.ApiControllers.Dto;

namespace Logo.Api.ApiControllers
{
    /// <summary>
    /// Business cards controller.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class BusinessCardsController : Controller
    {
        private readonly IBusinessCardsRepository businessCardsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="businessCardsRepository">Business cards repository.</param>
        /// <param name="userManager">User manager.</param>
        public BusinessCardsController(
            IBusinessCardsRepository businessCardsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.businessCardsRepository = businessCardsRepository;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get business cards for a current user.
        /// </summary>
        /// <returns>List of business cards of current user.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ApplicationUser appUser = await GetCurrentUserAsync();
            IEnumerable<BusinessCard> cards = await businessCardsRepository.GetByUserIdAsync(appUser.User.Id);
            IEnumerable<BusinessCardDto> dto = cards.Select(card => new BusinessCardDto(card));
            return Ok(dto);
        }

        /// <summary>
        /// Get bussiness card of current user via Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ApplicationUser appUser = await GetCurrentUserAsync();
            BusinessCard card = businessCardsRepository.Get(id);
            // User can not get cards of another users.
            if (card.CreatedBy.Id != appUser.User.Id)
            {
                return NotFound();
            }
            return Ok(new BusinessCardDto(card));
        }

        /// <summary>
        /// Add a business card.
        /// </summary>
        /// <param name="cardDto">Business card.</param>
        /// <returns>Created business card.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BusinessCardDto cardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Get current user
            ApplicationUser appUser = await GetCurrentUserAsync();
            var card = new BusinessCard
            {
                Data = cardDto.Data,
                ThumbnailUrl = cardDto.ThumbnailUrl,
                CreatedAt = DateTime.Now,
                CreatedBy = appUser.User,
            };
            await businessCardsRepository.SaveAsync(card);

            return Ok(new BusinessCardDto(card));
        }

        /// <summary>
        /// Update a business card.
        /// </summary>
        /// <param name="id">Updating business card Id.</param>
        /// <param name="cardDto">Business card data.</param>
        /// <returns>Updated business card.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]BusinessCardDto cardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cardDto.Id == 0)
            {
                throw new ArgumentException("Business card identifier doest not set.");
            }
            var card = businessCardsRepository.Get(cardDto.Id);
            if (card == null)
            {
                return NotFound();
            }
            // Update.
            card.Data = cardDto.Data;
            card.ThumbnailUrl = cardDto.ThumbnailUrl;
            // Save changes.
            await businessCardsRepository.SaveAsync(card);

            return Ok(new BusinessCardDto(card));
        }

        /// <summary>
        /// Delete a business card of current user via Id.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
