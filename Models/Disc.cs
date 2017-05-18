namespace Models
{
    public class Disc
    {
        public int ID { get; private set; }
        public int RelationID { get; private set; }
        public string Format { get; private set; }

        public Disc() { }

        public Disc(int id, int relationID, string format)
        {
            this.ID = id;
            this.RelationID = relationID;
            this.Format = format;
        }
    }
}