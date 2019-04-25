namespace Logo.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Logo.Domain.BusinessCards;
    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
    public class BusinessCardsRepository : IBusinessCardsRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        public BusinessCardsRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BusinessCard>> Get()
        {
            return await _context.BusinessCards.ToListAsync();
        }

        /// <inheritdoc />
        public BusinessCard Get(int id)
        {
            return _context.BusinessCards.Find(id);
        }

        /// <inheritdoc />
        public async Task SaveAsync(BusinessCard card)
        {
            if (card.Id == 0)
            {
                await _context.BusinessCards.AddAsync(card);
            }
            else
            {
                BusinessCard dbCard = await _context.BusinessCards.Where(bc => bc.Id == card.Id).SingleAsync();
                dbCard.Data = card.Data;
                dbCard.ThumbnailUrl = card.ThumbnailUrl;
            }
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BusinessCard>> GetByUserIdAsync(int userId)
        {
            return await _context.BusinessCards.Where(bc => bc.CreatedBy.Id == userId).ToListAsync();
        }
    }
}
 