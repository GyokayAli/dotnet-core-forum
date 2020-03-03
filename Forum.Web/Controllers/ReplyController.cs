using System;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class ReplyController : Controller
    {
        #region "Fields"

        private readonly IPost _postService;
        private readonly IApplicationUser _userService;

        private static UserManager<ApplicationUser> _userManager;
        #endregion

        #region "Constructor"

        public ReplyController(IPost postService, IApplicationUser userService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _userService = userService;
            _userManager = userManager;
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Creates a new post reply.
        /// </summary>
        /// <param name="id">The post id.</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,
                AuthorId = user.Id,
                AuthorName = User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorRating = user.Rating,
                IsAuthorAdmin = User.IsInRole("Admin"),
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                ForumImageUrl = post.Forum.ImageUrl,
                Created = DateTime.Now,
            };

            return View(model);
        }

        /// <summary>
        /// Adds a new post reply.
        /// </summary>
        /// <param name="model">The post reply model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildPostReply(model, user);

            await _postService.AddReply(reply);
            await _userService.UpdateUserRating(userId, typeof(PostReply));

            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Builds a new post reply.
        /// </summary>
        /// <param name="model">The post reply model.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private PostReply BuildPostReply(PostReplyModel model, ApplicationUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
        #endregion
    }
}