using System;
using System.Collections.Generic;
using FabrikamResidences_Activities.Models;
using Microsoft.EntityFrameworkCore;

namespace FabrikamResidences_Activities.Data
{
    public class PortalContext : DbContext
    {
        public PortalContext(DbContextOptions<PortalContext> options)
            : base(options)
        {

        }

        public DbSet<PortalActivity> PortalActivity { get; set; }
        public DbSet<Attendee> Attendee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DateTime now = DateTime.Now;
            int i = 1;

            modelBuilder.Entity<PortalActivity>()
                .HasData(new PortalActivity()
                {
                    Id = i++,
                    Name = "Bingo",
                    Description = "Come join us for an exciting game of Bingo with great prizes.",
                    Date = new DateTime(now.Year, now.Month, now.AddDays(2).Day, 12, 00, 00),

                },
                new PortalActivity()
                {
                    Id = i++,
                    Name = "Shuffleboard Competition",
                    Description = "Meet us at the Shuffleboard court!",
                    Date = new DateTime(now.Year, now.Month, now.AddDays(5).Day, 18, 00, 00)
                });

            i = 1;
            modelBuilder.Entity<Attendee>()
                .HasData(new 
                {
                    Id = i++,
                    PortalActivityId = 1,
                    FirstName = "Joe",
                    LastName = "Bingo",
                    Email = "Joe@Addict.com"
                },
                new 
                {
                    Id = i++,
                    PortalActivityId = 1,
                    FirstName = "john",
                    LastName = "doe",
                    Email = "jdoe@anonymous.com"
                },
                new 
                {
                    Id = i++,
                    PortalActivityId = 2,
                    FirstName = "Jill",
                    LastName = "Hill",
                    Email = "champ@shuffleboard.com"
                },
                new 
                {
                    Id = i++,
                    PortalActivityId = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@anonymous.com"
                }
            );
        }
    }
}