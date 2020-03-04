using Forum.Data;
using Forum.Web.Common;
using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
