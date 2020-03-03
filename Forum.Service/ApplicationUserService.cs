using Forum.Data;
using Forum.Data.Models;
using Forum.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region "Constructor"

        public ApplicationUserService (ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _dbContext.ApplicationUsers;
        }

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
        public ApplicationUser GetById(string id)
        {
            return GetAll()
                .FirstOrDefault(user => user.Id == id);
        }

        /// <summary>
        /// Updates the user rating.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="type">The type of action.</param>
        /// <returns></returns>
        public async Task UpdateUserRating(string userId, Type type)
        {
            var user = GetById(userId);
            user.Rating = Helpers.CalculateUserRating(type, user.Rating);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the user profile image.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <param name="uri">The new image uri.</param>
        /// <returns></returns>
        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
