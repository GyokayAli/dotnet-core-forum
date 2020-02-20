using System;
using System.Collections.Generic;
using System.Linq;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.Post;
using Forum.Web.Models.Reply;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;

        public PostController(IPost postService)
        {
            _postService = postService;
        }

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
    }
}