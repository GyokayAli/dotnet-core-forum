using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.Post;
using Forum.Web.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {

        #region "Fields"

        private readonly IPost _postService;
        private readonly IForum _forumService;
        private static UserManager<ApplicationUser> _userManager;

        #endregion

        #region "Constructor"

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
        }

        #endregion

        #region "Action Methods"

        /// <summary>
        /// Gets a post by id.
        /// </summary>
        /// <param name="id">The post id.</param>
        /// <returns></returns>
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies
            };

            return View(model);
        }

        /// <summary>
        /// Prepares an empty form and allows the creation of a new forum post.
        /// </summary>
        /// <param name="id">The forum id.</param>
        /// <returns></returns>
        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        /// <summary>
        /// Adds/creates a new post for forum.
        /// </summary>
        /// <param name="model">The new post model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var post = BuildPost(model, user);

            await _postService.Add(post);

            //TODO: Later to Implement User Rating Management

            return RedirectToAction("Index", "Post", new { id = post.Id });
        }

        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Builds a post model
        /// </summary>
        /// <param name="model">The new post model.</param>
        /// <param name="user">The application user.</param>
        /// <returns></returns>
        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }

        /// <summary>
        /// Builds a post replies model.
        /// </summary>
        /// <param name="replies">The collection of post replies.</param>
        /// <returns></returns>
        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content
            });
        }

        #endregion
    }
}