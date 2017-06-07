using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Data;
using Repository.Logic;

// ReSharper disable All

namespace MyCollectionDataBase.Controllers
{
    public class HomeController : Controller
    {
        private CollectionRepository collectionRepository = new CollectionRepository(new CollectionSQLContext());

        private int userID;

        [Route("")]
        [Route("Home")]
        [Route("[controller]/Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                return RedirectToAction("Collection");
            }
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

        [HttpGet("Collection")]
        [HttpGet("MyCollection")]
        public IActionResult Collection()
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    return View(collectionRepository.GetLists(userID));
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet("Thisisaverylongurlforapi")]
        public IActionResult GetAPI()
        {
            return View();
        }
    }
}