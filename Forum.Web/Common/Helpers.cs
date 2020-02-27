using Forum.Data.Models;
using Forum.Web.Models.Forum;

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
    }
}
