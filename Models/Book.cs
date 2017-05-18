using System.Collections.Generic;

namespace Models
{
    public class Book : Item
    {
        public string Format { get; private set; }
        public string Language { get; private set; }
        public int Pages { get; private set; }

        public Book()
        {
        }

        public Book(int id, string itemType, string title, string description, decimal price, int year, string country,
            string retailer, string exclusive, int limited, List<Image> images, List<Tag> tags, string format, string language,
            int pages) : base(id, itemType, title, description, price, year, country,
            retailer, exclusive, limited, images, tags)
        {
            this.Format = format;
            this.Language = language;
            this.Pages = pages;
        }
    }
}