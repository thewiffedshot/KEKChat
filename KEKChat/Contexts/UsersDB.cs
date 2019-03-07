using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KEKChat.Contexts
{
    public class UsersDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public UsersDB()
            : base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Message>().ToTable("messages");
            base.OnModelCreating(modelBuilder);
        }
    }
}