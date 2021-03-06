﻿using Eventures.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eventures.Data
{
    public class EventuresDbContext : IdentityDbContext<EventuresUser>
    {
        public EventuresDbContext(DbContextOptions<EventuresDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventuresUser>()
                   .HasAlternateKey(u => u.UCN);

            base.OnModelCreating(builder);
        }
    }
}
