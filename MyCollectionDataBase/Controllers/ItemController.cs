using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyCollectionDataBase.ViewModels;
using Repository.Data;
using Repository.Logic;

// ReSharper disable All

namespace MyCollectionDataBase.Controllers
{
    [Route("[controller]/[action]")]
    public class ItemController : Controller
    {
        private CollectionRepository collectionRepository = new CollectionRepository(new CollectionSQLContext());

        private int userID;

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    IEnumerable<Item> items = new List<Item>(collectionRepository.GetAllItems(userID));
                    switch (sortOrder)
                    {
                        case "name_desc":
                            items = items.OrderByDescending(s => s.Title).ThenByDescending(s => s.ItemType);
                            break;

                        case "type":
                            items = items.OrderBy(s => s.ItemType).ThenBy(s => s.Title);
                            break;

                        case "type_desc":
                            items = items.OrderByDescending(s => s.ItemType).ThenByDescending(s => s.Title);
                            break;

                        default:
                            items = items.OrderBy(s => s.Title).ThenBy(s => s.ItemType);
                            break;
                    }

                    return View(items);
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    Item item = collectionRepository.GetItem(id, userID);
                    return View(item);
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
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    ViewBag.Lists = collectionRepository.GetLists(userID).OrderBy(s => s.Name);
                    return View(new Item());
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
        public IActionResult Create(Item item)
        {
            //Couldn't find a better way to do this, because when I click submit, it doesn't give me the item I want to get.
            if (ModelState.IsValid)
            {
                item.ItemType = Request.Form["TypeRadio"];
                item.Title = Request.Form["Name"];
                item.Description = Request.Form["Description"];
                item.Price = Convert.ToDecimal(Request.Form["Price"]);
                item.Year = Convert.ToInt32(Request.Form["Year"]);
                item.Country = Request.Form["Country"];
                item.Retailer = Request.Form["Retailer"];
                item.Exclusive = Request.Form["Exclusive"];
                item.Limited = Convert.ToInt32(Request.Form["Limited"]);
                item.ListID = Convert.ToInt32(Request.Form["List"]);
                if (item.ItemType == "Other")
                {
                    item.ItemType = Request.Form["Type"];
                }

                foreach (string s in collectionRepository.SeperateString(Request.Form["Tags"]))
                {
                    item.tags.Add(new Tag() { Name = s, Type = "Tag" });
                }
                foreach (string s in collectionRepository.SeperateString(Request.Form["Genres"]))
                {
                    item.tags.Add(new Tag() { Name = s, Type = "Genre" });
                }
                foreach (string s in collectionRepository.SeperateString(Request.Form["Finishes"]))
                {
                    item.tags.Add(new Tag() { Name = s, Type = "Finish" });
                }
                int counter = 0;
                foreach (string s in collectionRepository.SeperateString(Request.Form["Images"]))
                {
                    string substring = "";
                    int lastIndex = s.LastIndexOf(".");
                    if (lastIndex > 0) { substring = s.Substring(lastIndex).ToLower(); }
                    if (substring == ".png" || substring == ".gif" || substring == ".jpg" || substring == ".jpeg")
                    {
                        counter++;
                        item.images.Add(new Image() { ItemPicture = s, Position = counter });
                    }
                }
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    switch (item.ItemType)
                    {
                        case "Book":
                            item.ItemBook = new Book();
                            item.ItemBook.Format = Request.Form["Format"];
                            item.ItemBook.Language = Request.Form["Language"];
                            item.ItemBook.Pages = Convert.ToInt32(Request.Form["Pages"]);
                            collectionRepository.SaveItem(item, item.ItemBook);
                            break;

                        case "Case":
                            item.ItemCase = new Case();
                            item.ItemCase.CaseType = Request.Form["CaseType"];
                            item.ItemCase.Cover = Request.Form["Cover"];
                            if (Convert.ToInt32(Request.Form["4K Blu-Ray"]) != 0) { item.ItemCase.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["4K Blu-Ray"]), Format = "4K Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["3D Blu-Ray"]) != 0) { item.ItemCase.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["3D Blu-Ray"]), Format = "3D Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["Blu-Ray"]) != 0) { item.ItemCase.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["Blu-Ray"]), Format = "Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["DVD"]) != 0) { item.ItemCase.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["DVD"]), Format = "DVD" }); }
                            if (Convert.ToInt32(Request.Form["CD"]) != 0) { item.ItemCase.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["CD"]), Format = "CD" }); }
                            collectionRepository.SaveItem(item, item.ItemCase);
                            break;

                        case "Media":
                            item.ItemMedia = new Media();
                            item.ItemMedia.MediaType = Request.Form["MediaType"];
                            item.ItemMedia.Runtime = Convert.ToInt32(Request.Form["Runtime"]);
                            item.ItemMedia.Platform = Request.Form["Platform"];
                            if (Convert.ToInt32(Request.Form["4K Blu-Ray"]) != 0) { item.ItemMedia.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["4K Blu-Ray"]), Format = "4K Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["3D Blu-Ray"]) != 0) { item.ItemMedia.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["3D Blu-Ray"]), Format = "3D Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["Blu-Ray"]) != 0) { item.ItemMedia.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["Blu-Ray"]), Format = "Blu-Ray" }); }
                            if (Convert.ToInt32(Request.Form["DVD"]) != 0) { item.ItemMedia.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["DVD"]), Format = "DVD" }); }
                            if (Convert.ToInt32(Request.Form["CD"]) != 0) { item.ItemMedia.Discs.Add(new Disc() { Amount = Convert.ToInt32(Request.Form["CD"]), Format = "CD" }); }
                            collectionRepository.SaveItem(item, item.ItemMedia);
                            break;

                        default:
                            collectionRepository.SaveItem(item);
                            break;
                    }
                    return RedirectToAction("Details", new { id = item.ID });
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    return View(collectionRepository.GetItem(id, userID));
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
        public IActionResult Edit(List list)
        {
            if (ModelState.IsValid)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    //collectionRepository.SaveItem(list, userID);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return View("Error", "Database");
                }
            }
            return View(list);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.Get("Username") != null)
            {
                userID = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
                try
                {
                    collectionRepository.DeleteItem(id, userID);
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

        public IActionResult TypeDetails(string filter, Item item)
        {
            ViewBag.Finishes = collectionRepository.GetFinishes().OrderBy(s => s);
            ViewBag.Genres = collectionRepository.GetGenres().OrderBy(s => s);

            switch (filter)
            {
                case "Book":
                    return PartialView("_BookType", item.ItemBook = new Book());
                    break;

                case "Case":
                    return PartialView("_CaseType", item.ItemCase = new Case());
                    break;

                case "Media":
                    return PartialView("_MediaType", item.ItemMedia = new Media());
                    break;

                default:
                    return Content("");
                    break;
            }
        }
    }
}