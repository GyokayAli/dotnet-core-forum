using System.Linq;
using Forum.Data;
using Forum.Web.Common;
using Forum.Web.Models.Post;
using Forum.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class SearchController : Controller
    {
        #region "Fields"

        private readonly IPost _postService;
        #endregion

        #region "Constructor"

        public SearchController(IPost postService)
        {
            _postService = postService;
        }
        #endregion

        #region "Action Methods"

        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        public IActionResult Results(string searchQuery)
        {
            var posts = _postService.GetFilteredPosts(searchQuery);
            var areNoResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());

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

            var model = new SearchResultModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults
            };

            return View(model);
        }

        /// <summary>
        /// Wrapper to handle search requests.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
        #endregion
    }
}