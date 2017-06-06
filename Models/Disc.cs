namespace Models
{
    public class Disc
    {
        public int ID { get; set; }
        public string Format { get; set; }
        public int RelationID { get; set; }
        public int CaseID { get; set; }
        public int MediaID { get; set; }
        public int Amount { get; set; }
    }
}