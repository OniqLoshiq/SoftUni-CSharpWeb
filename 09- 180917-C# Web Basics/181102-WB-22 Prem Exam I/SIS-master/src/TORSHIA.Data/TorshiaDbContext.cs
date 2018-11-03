using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TORSHIA.Models;
using TORSHIA.Models.Enums;

namespace TORSHIA.Data
{
    public class TorshiaDbContext : DbContext
    {
        public TorshiaDbContext() {}

        public TorshiaDbContext(DbContextOptions options) 
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<TaskSector> TaskSectors { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConfigString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(r => r.Role).HasConversion(new EnumToStringConverter<UserRole>());

            modelBuilder.Entity<Report>().Property(r => r.Status).HasConversion(new EnumToStringConverter<Status>());
            
            modelBuilder.Entity<TaskSector>().HasKey(ts => new { ts.TaskId, ts.SectorId });
        }
    }
}
