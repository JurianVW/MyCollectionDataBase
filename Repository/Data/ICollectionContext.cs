using System.Collections.Generic;
using Models;

namespace Repository.Data
{
    public interface ICollectionContext
    {
        void saveList(List list);

        List getList();

        void saveItem(Item item);

        Item getItem();
    }
}