using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models;
using Forum.Web.Models.Home;
using Forum.Data;
using System.Linq;
using Forum.Web.Models.Post;
using Forum.Data.Models;
using Forum.Web.Models.Forum;
using Forum.Web.Common;

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

        #region "Methods"

        /// <summary>
        /// Prepares a model and gets the home page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var model = Helpers.BuildHomeIndexModel(_postService);
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
    }
}
