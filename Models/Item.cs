using System.Collections.Generic;

namespace Models
{
    public class Item
    {
        public int ID { get; private set; }
        public string ItemType { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Year { get; private set; }
        public string Country { get; private set; }
        public string Retailer { get; private set; }
        public string Exclusive { get; private set; }
        public int Limited { get; private set; }
        public readonly List<Image> images;
        public readonly List<Tag> tags;

        public Item()
        {
        }

        public Item(int id, string itemType, string title, string description, decimal price, int year, string country,
            string retailer, string exclusive, int limited, List<Image> images, List<Tag> tags)
        {
            this.ID = id;
            this.ItemType = itemType;
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.Year = year;
            this.Country = country;
            this.Retailer = retailer;
            this.Exclusive = exclusive;
            this.Limited = limited;
            this.images.AddRange(images);
            this.tags.AddRange(tags);
        }
    }
}