using Microsoft.AspNetCore.Mvc;

// ReSharper disable All

namespace MyCollectionDataBase.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Home")]
        [Route("[controller]/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("About")]
        [Route("[controller]/About")]
        public IActionResult About()
        {
            return View();
        }

        [Route("Contact")]
        [Route("[controller]/Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("Error");
        }
    }
}