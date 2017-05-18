using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Models;
using Repository.Data;
using Repository.Logic;

namespace MyCollectionDataBase.Controllers
{
    public class UserController : Controller
    {
        private UserRepository userRepository = new UserRepository(new UserSQLContext());

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            if (ModelState.IsValid)
            {
                if (userRepository.CheckUserLogin(u))
                {
                    HttpContext.Session.SetString("Username", u.Username);
                    return RedirectToRoute("Profile", new { action = "Profile", u.Username });
                }
            }
            u.Password = "";
            return View(u);
        }

        //[HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Register(User u)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveUser(u);//need to impliment
            }
            u.Password = "";
            u.ConfirmationPassword = "";
            return View(u);
        }

        public IActionResult Profile(string username = "")
        {
            if (HttpContext.Session.GetString("Username") == username)
            {
                return View();
            }

            return RedirectToAction("Login", "User");
        }
    }
}