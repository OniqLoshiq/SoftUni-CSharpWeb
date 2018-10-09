using System.Collections.Generic;
using System.Linq;

namespace IRunesModels
{
    public class Album : BaseEntity<string>
    {
        private const decimal ReducedAlbumPricePercentage = 0.87m;

        public Album()
        {
            this.Tracks = new List<Track>();
        }

        public string Name { get; set; }

        public string CoverUrl { get; set; }

        public decimal Price => this.Tracks.Sum(t => t.Price) * ReducedAlbumPricePercentage;

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }

    }
}
