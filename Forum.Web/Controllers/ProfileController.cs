using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Forum.Web.Controllers
{
    public class ProfileController : Controller
    {
        #region "Fields"

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;

        #endregion

        #region "Constructor"

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Displays the user profile page.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            // Connect to Azure Storage Account Container
            // Get Blob Container

            // Parse the Content Disposition response header
            // Grab the file name

            // Get a reference to a Block Blob  
            // On that Block Blob, upload our file <-- file uploaded to the cloud

            // Set the user's profile image to the URI 
            // Redirect to the user's profile image

            throw new NotImplementedException();
        }

        #endregion
    }
}