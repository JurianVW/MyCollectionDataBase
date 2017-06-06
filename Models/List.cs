using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class List
    {
        public int ID { get; set; }
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; } = "";
        public IEnumerable<Item> items = new List<Item>();
    }
}