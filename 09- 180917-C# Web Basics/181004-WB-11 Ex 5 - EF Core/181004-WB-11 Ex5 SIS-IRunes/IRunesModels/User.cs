using System.Collections.Generic;

namespace IRunesModels
{
    public class User : BaseEntity<string>
    {
        public User()
        {
            this.Albums = new List<Album>();
        }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
