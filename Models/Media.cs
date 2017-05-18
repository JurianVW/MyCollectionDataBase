using System.Collections.Generic;

namespace Models
{
    public class Media : Item
    {
        public string MediaType { get; private set; }
        public string Cover { get; private set; }

        public Media()
        {
        }

        public Media(int id, string itemType, string title, string description, decimal price, int year, string country,
            string retailer, string exclusive, int limited, List<Image> images, List<Tag> tags, string mediaType,
            string cover) : base(id, itemType, title, description, price, year, country,
            retailer, exclusive, limited, images, tags)
        {
            this.MediaType = mediaType;
            this.Cover = cover;
        }
    }
}