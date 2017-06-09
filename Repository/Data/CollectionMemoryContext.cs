using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Repository.Data
{
    public class CollectionMemoryContext : ICollectionContext
    {
        public bool CheckMyItem(int itemID, int userID)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int itemID)
        {
            throw new NotImplementedException();
        }

        public void DeleteList(int listID)
        {
            throw new NotImplementedException();
        }

        public List<Disc> GetEmptyDiscs()
        {
            throw new NotImplementedException();
        }

        public List<string> GetFinishes()
        {
            throw new NotImplementedException();
        }

        public List<string> GetGenres()
        {
            throw new NotImplementedException();
        }

        public Item GetItem(int itemID, int userID)
        {
            throw new NotImplementedException();
        }

        public List<Item> GetItems(int userID)
        {
            throw new NotImplementedException();
        }

        public List<Item> GetItems(int listID, int userID)
        {
            throw new NotImplementedException();
        }

        public List GetList(int listID, int userID)
        {
            List<List> lists = new List<List>() { new List { ID = 1, UserID = 1 } };
            foreach (List l in lists)
            {
                if (listID == l.ID && userID == l.UserID)
                {
                    return l;
                }
            }
            return new List();
        }

        public List<List> GetLists(int userID)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item, Book itemBook)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item, Case itemCase)
        {
            throw new NotImplementedException();
        }

        public void SaveItem(Item item, Media itemMedia)
        {
            throw new NotImplementedException();
        }

        public void SaveList(List list, int userID)
        {
            throw new NotImplementedException();
        }
    }
}