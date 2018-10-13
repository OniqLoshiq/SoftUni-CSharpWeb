namespace IRunesModels
{
    public class Track : BaseEntity<string>
    {
        public string Name { get; set; }

        public string LinkUrl { get; set; }

        public decimal Price { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}
