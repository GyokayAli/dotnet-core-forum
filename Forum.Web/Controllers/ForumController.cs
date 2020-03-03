using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Common;
using Forum.Web.Models.Forum;
using Forum.Web.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Forum.Web.Controllers
{
    public class ForumController : Controller
    {
        #region "Fields"

        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        #endregion

        #region "Constructor"

        public ForumController(IForum forumService, IPost postService, IUpload uploadService, IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
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
                    Description = forum.Description,
                    ImageUrl = forum.ImageUrl,
                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
                    HasRecentPost = _forumService.HasRecentPost(forum.Id)
                });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Name)
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

        /// <summary>
        /// Creates an empty forum model.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        /// <summary>
        /// Creates a new forum.
        /// </summary>
        /// <param name="model">The new forum.</param>
        /// <returns></returns>
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/users/default.png";

            if (model.ImageUpload != null)
            {
                CloudBlockBlob blockBlob = UploadForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }

            var forum = new ForumEntity
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Create(forum);

            return RedirectToAction("Index", "Forum");
        }
        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Upload forum image to Azure storage.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private CloudBlockBlob UploadForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");

            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var fileName = contentDisposition.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStreamAsync(file.OpenReadStream()).Wait();

            return blockBlob;
        }
        #endregion
    }
}