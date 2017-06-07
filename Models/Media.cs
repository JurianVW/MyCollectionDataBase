using System.Collections.Generic;

namespace Models
{
    public class Media
    {
        public string MediaType { get; set; }
        public int Runtime { get; set; }
        public string Platform { get; set; }
        public List<Disc> Discs { get; set; } = new List<Disc>();
    }
}