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

        private readonly ApplicationDbContext _dbContex;

        #endregion

        #region "Constructor" 

        public ForumService(ApplicationDbContext dbContext)
        {
            _dbContex = dbContext;
        }

        #endregion

        #region "Public Methods"

        public Task Create(ForumEntity forum)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all forums available.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumEntity> GetAll()
        {
            return _dbContex.Forums
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
            var forum = _dbContex.Forums.Where(f => f.Id == id)
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
