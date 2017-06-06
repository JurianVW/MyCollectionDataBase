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

        public void DeleteList(int listID)
        {
            context.DeleteList(listID);
        }

        public bool CheckMyList(int listID, int userID)
        {
            if (context.GetList(listID, userID).ID == 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckMyItem(int itemID, int userID)
        {
            if (context.GetItem(itemID, userID).ID == 0)
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
            return context.GetAllItems(userID);
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
    }
}