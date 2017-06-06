using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Data;
using Repository.Logic;

// ReSharper disable All

namespace MyCollectionDataBase.Controllers
{
    [Route("[controller]/[action]")]
    public class ListController : Controller
    {
        private CollectionRepository collectionRepository = new CollectionRepository(new CollectionSQLContext());

        private int userID;

        public IActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    IEnumerable<List> lists = new List<List>(collectionRepository.GetLists(userID));
                    switch (sortOrder)
                    {
                        case "name_desc":
                            lists = lists.OrderByDescending(s => s.Name);
                            break;

                        default:
                            lists = lists.OrderBy(s => s.Name);
                            break;
                    }

                    return View(lists);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult Details(int id, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    List list = collectionRepository.GetList(id, userID);
                    switch (sortOrder)
                    {
                        case "name_desc":
                            list.items = list.items.OrderByDescending(s => s.Title).ThenByDescending(s => s.ItemType);
                            break;

                        case "type":
                            list.items = list.items.OrderBy(s => s.ItemType).ThenBy(s => s.Title);
                            break;

                        case "type_desc":
                            list.items = list.items.OrderByDescending(s => s.ItemType).ThenByDescending(s => s.Title);
                            break;

                        default:
                            list.items = list.items.OrderBy(s => s.Title).ThenBy(s => s.ItemType);
                            break;
                    }
                    return View(list);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                return View(new List());
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public IActionResult Create(List l)
        {
            if (ModelState.IsValid)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    collectionRepository.SaveList(l, userID);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return View(l);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    if (collectionRepository.CheckMyList(id, userID))
                    {
                        return View(collectionRepository.GetList(id, userID));
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public IActionResult Edit(List l)
        {
            if (ModelState.IsValid)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    collectionRepository.SaveList(l, userID);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return View(l);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    if (collectionRepository.CheckMyList(id, userID))
                    {
                        collectionRepository.DeleteList(id);
                    }
                    return RedirectToAction("Index");
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