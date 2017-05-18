namespace Models
{
    public class Tag
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public Tag()
        {
        }

        public Tag(int id, string name, string type)
        {
            this.ID = id;
            this.Name = name;
            this.Type = type;
        }
    }
}