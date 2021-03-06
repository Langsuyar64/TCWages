using System;
using Microsoft.EntityFrameworkCore;
using TCWages.API.Models;

namespace TCWages.API.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) {}
        public DbSet<Value> Values { get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>()
            .HasKey(k => new {k.LikerID, k.LikeeID});

            builder.Entity<Like>()
            .HasOne(u => u.Likee)
            .WithMany(u => u.Likers)
            .HasForeignKey(u => u.LikeeID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
            .HasOne(u => u.Liker)
            .WithMany(u => u.Likees)
            .HasForeignKey(u => u.LikerID)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}