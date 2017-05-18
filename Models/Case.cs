using System.Collections.Generic;

namespace Models
{
    public class Case : Item
    {
        public string CaseType { get; private set; }
        public int Runtime { get; private set; }
        public string Platform { get; private set; }
        public readonly List<Disc> discs;

        public Case()
        {
        }

        public Case(int id, string itemType, string title, string description, decimal price, int year, string country,
            string retailer, string exclusive, int limited, List<Image> images, List<Tag> tags, string caseType, int runtime, string platform, List<Disc> discs) : base(id, itemType, title, description, price, year, country,
            retailer, exclusive, limited, images, tags)
        {
            this.CaseType = caseType;
            this.Runtime = runtime;
            this.Platform = platform;
            this.discs.AddRange(discs);
        }
    }
}