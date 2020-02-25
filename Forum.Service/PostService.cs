using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Post post)
        {
            _dbContext.Add(post);
            await _dbContext.SaveChangesAsync(); 
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Post> GetAll()
        {
            return _dbContext.Posts
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        /// <summary>
        /// Gets a post by id.
        /// </summary>
        /// <param name="id">The post id.</param>
        /// <returns></returns>
        public Post GetById(int id)
        {
            return _dbContext.Posts.Where(post => post.Id == post.Id)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .First();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get latest number of posts.
        /// </summary>
        /// <param name="numberOfPosts">Number of posts to return.</param>
        /// <returns></returns>
        public IEnumerable<Post> GetLatestPosts(int numberOfPosts)
        {
            return GetAll()
                .OrderByDescending(post => post.Created)
                .Take(numberOfPosts);
        }

        /// <summary>
        /// Gets posts by forum.
        /// </summary>
        /// <param name="id">The forum id.</param>
        /// <returns></returns>
        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _dbContext.Forums
                .Where(forum => forum.Id == id).First()
                .Posts;
        }
    }
}
