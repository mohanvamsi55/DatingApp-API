using System.Collections.Generic;
using DatingApp.Api.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.DataAccess
{
    public class DatingAppDbContext : DbContext
    {
        public DatingAppDbContext(DbContextOptions<DatingAppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(new List<AppUser>
            {
                new AppUser
                {
                    Id = 1,
                    UserName = "Mohana Vamsi"
                },
                new AppUser
                {
                    Id = 2,
                    UserName = "Sushmitha"
                },
                new AppUser
                {
                    Id = 3,
                    UserName = "Sush Vamc"
                }
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
