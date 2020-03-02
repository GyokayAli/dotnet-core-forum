using Forum.Data;
using Forum.Data.Models;
using Forum.Web.Models.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Forum.Web.Controllers
{
    public class ProfileController : Controller
    {
        #region "Fields"

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        #endregion

        #region "Constructor"

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
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
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");

            // Get Blob Container
            var container = _uploadService.GetBlobContainer(connectionString);

            // Parse the Content Disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            // Grab the file name
            var fileName = contentDisposition.FileName.Trim('"');

            // Get a reference to a Block Blob  
            var blockBlob = container.GetBlockBlobReference(fileName);

            // On that Block Blob, upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            // Set the user's profile image to the URI 
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            // Redirect to the user's profile image
            return RedirectToAction("Detail", "Profile", new { id = userId });
        }

        #endregion
    }
}