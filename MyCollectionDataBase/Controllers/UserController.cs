using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyCollectionDataBase.ViewModels;
using Repository.Data;
using Repository.Logic;

// ReSharper disable All

namespace MyCollectionDataBase.Controllers
{
    public class UserController : Controller
    {
        private UserRepository userRepository = new UserRepository(new UserSQLContext());
        private CollectionRepository collectionRepository = new CollectionRepository(new CollectionSQLContext());

        private int userID;

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View(new UserLogin());
            }
            return RedirectToAction("Profile");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin ul)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userRepository.CheckUserLogin(ul))
                    {
                        HttpContext.Session.SetInt32("UserID", ul.ID);
                        HttpContext.Session.SetString("Username", ul.Username);
                        return RedirectToAction("Profile", new { ul.Username });
                    }
                    ViewBag.Message = "Username and/or Password are incorrect.";
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            ul.Password = "";
            return View(ul);
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View(new UserRegister());
            }
            return RedirectToAction("Profile");
        }

        [HttpPost("Register")]
        public IActionResult Register(UserRegister ur)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!userRepository.CheckEmailAvailability(ur))
                    {
                        ViewBag.MessageEmail = "Email Address Already in use.";
                    }
                    if (!userRepository.CheckUsernameAvailability(ur))
                    {
                        ViewBag.MessageUsername = "Username Already in use.";
                    }
                    if (userRepository.CheckEmailAvailability(ur) && userRepository.CheckUsernameAvailability(ur))
                    {
                        userRepository.SaveUser(ur);
                        HttpContext.Session.SetInt32("UserID", ur.ID);
                        HttpContext.Session.SetString("Username", ur.Username);
                        return RedirectToAction("Profile", new { ur.Username });
                    }
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            ur.Password = "";
            ur.ConfirmationPassword = "";
            return View(ur);
        }

        //Needs to be fixed to watch friends profile
        [HttpGet("[controller]/{username}")]
        [HttpGet("[controller]/{username}/Profile")]
        public IActionResult Profile(string username = "")
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                try
                {
                    if (HttpContext.Session.GetString("Username") == username)
                    {
                        ViewBag.User = userRepository.GetUser(username);
                        return View(collectionRepository.GetLists(ViewBag.User.ID));
                    }
                    foreach (string friend in userRepository.GetUserFriendUsernames(HttpContext.Session.GetString("Username")))
                    {
                        if (friend == username)
                        {
                            ViewBag.User = userRepository.GetUser(username);
                            return View(collectionRepository.GetLists(ViewBag.User.ID));
                        }
                    }
                    return RedirectToAction("Profile", new { username = HttpContext.Session.GetString("Username") });
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet("[controller]/{username}/Settings")]
        public IActionResult Settings()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                try
                {
                    User u = userRepository.GetUser(HttpContext.Session.GetString("Username"));
                    UserSettings us = new UserSettings
                    {
                        ID = u.ID,
                        Username = u.Username,
                        Password = u.Password,
                        EmailAddress = u.EmailAddress,
                        ProfilePicture = u.ProfilePicture
                    };
                    HttpContext.Session.SetString("CurrentUsername", us.Username);
                    HttpContext.Session.SetString("CurrentEmail", us.EmailAddress);
                    return View(us);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost("[controller]/{username}/Settings")]
        public IActionResult Settings(UserSettings us)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool valid = true;
                    string currentEmail = HttpContext.Session.GetString("CurrentEmail");
                    string currentUsername = HttpContext.Session.GetString("CurrentUsername");
                    if (!userRepository.CheckEmailAvailability(us) && currentEmail.Replace(" ", "") != us.EmailAddress.Replace(" ", ""))
                    {
                        valid = false;
                        ViewBag.MessageEmail = "Email Address Already in use.";
                    }
                    if (!userRepository.CheckUsernameAvailability(us) && currentUsername.Replace(" ", "") != us.Username.Replace(" ", ""))
                    {
                        valid = false;
                        ViewBag.MessageUsername = "Username Already in use.";
                    }
                    if (valid)
                    {
                        us.ID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                        userRepository.SaveUser(us);
                        HttpContext.Session.SetString("Username", us.Username);
                        return RedirectToAction("Profile", new { username = HttpContext.Session.GetString("Username") });
                    }
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return View(us);
        }

        [HttpGet("[controller]/{username}/Friends")]
        public IActionResult Friends()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                try
                {
                    IEnumerable<UserFriend> friends = userRepository.GetUserFriends(HttpContext.Session.GetString("Username"));
                    friends = friends.OrderBy(s => s.Username);
                    ViewBag.Username = HttpContext.Session.GetString("Username");
                    return View(friends);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [Route("[controller]/{username}/FriendAction")]
        public IActionResult FriendAction(int id, string friendAction, string addFriendUsername)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    switch (friendAction)
                    {
                        case "Unfriend":
                            userRepository.Unfriend(userID, id);
                            break;

                        case "Accept":
                            userRepository.RespondFriendRequest(userID, id, true);
                            break;

                        case "Decline":
                            userRepository.RespondFriendRequest(userID, id, false);
                            break;

                        case "SendRequest":
                            userRepository.SendFriendRequest(userID, addFriendUsername, HttpContext.Session.GetString("Username"));
                            break;
                    }
                    //unfriend action
                    return RedirectToAction("Friends", "User");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }
    }
}