using IRunesModels;
using Microsoft.EntityFrameworkCore;

namespace IRunesData
{
    public class IRunesDbContext : DbContext
    {
        public IRunesDbContext() {}
        public IRunesDbContext(DbContextOptions options) 
            : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }
    }
}
