using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Service
{
    public class ForumService : IForum
    {
        #region "Fields"

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region "Constructor" 

        public ForumService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Create a new forum.
        /// </summary>
        /// <param name="forum">The forum.</param>
        /// <returns></returns>
        public async Task Create(ForumEntity forum)
        {
            _dbContext.Add(forum);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a forum.
        /// </summary>
        /// <param name="forumId">The forum id.</param>
        /// <returns></returns>
        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _dbContext.Remove(forum);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all forums available.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumEntity> GetAll()
        {
            return _dbContext.Forums
                .Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a forum by id.
        /// </summary>
        /// <param name="id">The forum id.</param>
        /// <returns></returns>
        public ForumEntity GetById(int id)
        {
            var forum = _dbContext.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts).ThenInclude(p => p.User)
                .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(r => r.User)
                .FirstOrDefault();

            return forum;
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
