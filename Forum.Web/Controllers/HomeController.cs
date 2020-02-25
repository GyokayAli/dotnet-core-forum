using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models;
using Forum.Web.Models.Home;
using Forum.Data;
using System.Linq;
using Forum.Web.Models.Post;
using Forum.Data.Models;
using Forum.Web.Models.Forum;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        #region "Fields"

        private readonly IPost _postService;

        #endregion

        #region "Constructor"

        public HomeController(IPost postService)
        {
            _postService = postService;
        }

        #endregion

        #region "Action Methods"

        /// <summary>
        /// Prepares a model and gets the home page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Helps to build the Home Index Model.
        /// </summary>
        /// <returns></returns>
        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10);

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

        /// <summary>
        /// Gets forum listing model for post.
        /// </summary>
        /// <param name="post">The post</param>
        /// <returns></returns>
        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

        #endregion
    }
}
