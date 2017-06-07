using System.Collections.Generic;

namespace Models
{
    public class Case
    {
        public string CaseType { get; set; }
        public string Cover { get; set; }
        public List<Disc> Discs { get; set; } = new List<Disc>();
    }
}