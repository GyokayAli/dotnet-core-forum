using System.Collections.Generic;
using System.Linq;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Common;
using Forum.Web.Models.Forum;
using Forum.Web.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class ForumController : Controller
    {
        #region "Fields"

        private readonly IForum _forumService;
        private readonly IPost _postService;

        #endregion

        #region "Constructor"

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        #endregion

        #region "Action Methods"

        /// <summary>
        /// Gets all forum topics.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        /// <summary>
        /// Gets a forum topic by id and/or search query.
        /// </summary>
        /// <param name="id">The topic id.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = new List<Post>();

            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = Helpers.BuildForumListing(post)
            });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = Helpers.BuildForumListing(forum)
            };

            return View(model);
        }

        /// <summary>
        /// Wrapper method to post a search query.
        /// </summary>
        /// <param name="id">The forum id.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        #endregion
    }
}