using System.Collections.Generic;

namespace Models
{
    public class Item
    {
        public int ID { get; set; }
        public int ListID { get; set; }
        public string ItemType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public string Retailer { get; set; }
        public string Exclusive { get; set; }
        public int Limited { get; set; }
        public List<Image> images = new List<Image>();
        public List<Tag> tags = new List<Tag>();

        public Book ItemBook { get; set; }
        public Case ItemCase { get; set; }
        public Media ItemMedia { get; set; }
    }
}