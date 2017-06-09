using System;
using System.Collections.Generic;
using System.Reflection;
using Models;
using Repository.Data;

namespace Repository.Logic
{
    public class CollectionRepository
    {
        private ICollectionContext context;

        public CollectionRepository(ICollectionContext context)
        {
            this.context = context;
        }

        public List GetList(int listID, int userID)
        {
            return context.GetList(listID, userID);
        }

        public List<List> GetLists(int userID)
        {
            return context.GetLists(userID);
        }

        public void SaveList(List list, int userID)
        {
            context.SaveList(list, userID);
        }

        public void DeleteList(int listID, int userID)
        {
            if (CheckMyList(listID, userID))
            {
                context.DeleteList(listID);
            }
        }

        public bool CheckMyList(int listID, int userID)
        {
            if (context.GetList(listID, userID).ID == 0)
            {
                return false;
            }
            return true;
        }

        public Item GetItem(int itemID, int userID)
        {
            return context.GetItem(itemID, userID);
        }

        public List<Item> GetAllItems(int userID)
        {
            return context.GetItems(userID);
        }

        public void SaveItem(Item item)
        {
            context.SaveItem(item);
        }

        public void SaveItem(Item item, Book itemBook)
        {
            context.SaveItem(item, itemBook);
        }

        public void SaveItem(Item item, Case itemCase)
        {
            context.SaveItem(item, itemCase);
        }

        public void SaveItem(Item item, Media itemMedia)
        {
            context.SaveItem(item, itemMedia);
        }

        public void DeleteItem(int itemID, int userID)
        {
            if (context.CheckMyItem(itemID, userID))
            {
                context.DeleteItem(itemID);
            }
        }

        public List<Disc> GetEmptyDiscs()
        {
            return context.GetEmptyDiscs();
        }

        public List<string> GetFinishes()
        {
            return context.GetFinishes();
        }

        public List<string> GetGenres()
        {
            return context.GetGenres();
        }

        public List<string> SeperateString(string list)
        {
            List<string> result = new List<string>();
            if (list != null)
            {
                list = list.Trim() + ",";
                while (list.Length > 0)
                {
                    int index = list.IndexOf(",");
                    string item = list.Substring(0, index).Trim();
                    if (item.Length > 0) { result.Add(item); };
                    list = list.Remove(0, index + 1).Trim();
                }
            }
            return result;
        }
    }
}