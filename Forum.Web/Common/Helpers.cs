using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.Forum;
using Forum.Web.Models.Home;
using Forum.Web.Models.Post;
using System;
using System.Linq;

namespace Forum.Web.Common
{
    public class Helpers
    {
        /// <summary>
        /// Builds a forum listing model.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <returns></returns>
        public static ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        /// <summary>
        /// Builds a forum listing model.
        /// </summary>
        /// <param name="forum">The forum.</param>
        /// <returns></returns>
        public static ForumListingModel BuildForumListing(ForumEntity forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }

        /// <summary>
        /// Gets forum listing model for post.
        /// </summary>
        /// <param name="post">The post</param>
        /// <returns></returns>
        public static ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

        /// <summary>
        /// Helps to build the Home Index Model.
        /// </summary>
        /// <param name="post">The post service.</param>
        /// <returns></returns>
        public static HomeIndexModel BuildHomeIndexModel(IPost postService)
        {
            var latestPosts = postService.GetLatestPosts(10);

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = string.Empty
            };
        }
    }
}
