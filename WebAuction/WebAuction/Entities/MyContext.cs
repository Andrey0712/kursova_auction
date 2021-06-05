using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Entities
{
    public class MyContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lot { get; set; }
        public DbSet<UserLot> UserLot { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
            
        //    optionsBuilder.UseNpgsql("Server=91.238.103.134;Port=5623;Database=dbkondr;Username=andrey;Password=$5$BG)dfffhgjUbe}xk");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLot>(userRole =>
            {
                userRole.HasKey(tp => new { tp.UserId, tp.LotId });

                userRole.HasOne(tp => tp.User)
                    .WithMany(t => t.UserLot)
                    .HasForeignKey(tp => tp.UserId)
                    .IsRequired();

                userRole.HasOne(tp => tp.Lot)
                    .WithMany(t => t.UserLot)
                    .HasForeignKey(tp => tp.LotId)
                    .IsRequired();
            });
        }
    }
}
