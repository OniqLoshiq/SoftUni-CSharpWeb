using Chushka.Models;
using Chushka.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chushka.Web.Data
{
    public class ChushkaDbContext : IdentityDbContext<ChushkaUser>
    {
        public ChushkaDbContext(DbContextOptions<ChushkaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(p => p.Type).HasConversion(new EnumToStringConverter<ProductType>());

            builder.Entity<Order>().HasOne(o => o.Product).WithMany(o => o.Orders).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>().HasOne(o => o.Client).WithMany(o => o.Orders).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
