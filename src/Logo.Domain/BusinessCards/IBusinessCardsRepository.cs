namespace Logo.Domain.BusinessCards
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Business cards repository.
    /// </summary>
    public interface IBusinessCardsRepository
    {
        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id">Business card identity.</param>
        /// <returns></returns>
        BusinessCard Get(int id);

        /// <summary>
        /// Get all business cards.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BusinessCard>> Get();

        /// <summary>
        /// Save business card.
        /// </summary>
        /// <param name="card">Business card for save.</param>
        /// <returns></returns>
        Task SaveAsync(BusinessCard card);

        /// <summary>
        /// Get business cards created by passed user.
        /// </summary>
        /// <param name="userId">User identity</param>
        /// <returns></returns>
        Task<IEnumerable<BusinessCard>> GetByUserIdAsync(int userId);
    }
}
