using System.Collections.Generic;

namespace Models
{
    public class List
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public readonly List<Item> items;

        public List()
        {
        }

        public List(int id, string name, string description, List<Item> items)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.items.AddRange(items);
        }
    }
}