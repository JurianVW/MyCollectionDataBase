namespace Models
{
    public class Image
    {
        public int ID { get; private set; }
        public string ItemPicture { get; private set; }
        public int Position { get; private set; }

        public Image()
        {
        }

        public Image(int id, string itemPicture, int position)
        {
            this.ID = id;
            this.ItemPicture = itemPicture;
            this.Position = position;
        }
    }
}