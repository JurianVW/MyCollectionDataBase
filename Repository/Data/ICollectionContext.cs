using System.Collections.Generic;
using Models;

namespace Repository.Data
{
    public interface ICollectionContext
    {
        bool CheckMyItem(int itemID, int userID);

        void SaveList(List list, int userID);

        List GetList(int listID, int userID);

        List<List> GetLists(int userID);

        void DeleteList(int listID);

        void SaveItem(Item item);

        void SaveItem(Item item, Book itemBook);

        void SaveItem(Item item, Case itemCase);

        void SaveItem(Item item, Media itemMedia);

        Item GetItem(int itemID, int userID);

        List<Item> GetAllItems(int userID);

        List<Item> GetItems(int listID, int userID);

        void DeleteItem(int itemID);

        List<string> GetFinishes();

        List<string> GetGenres();
    }
}