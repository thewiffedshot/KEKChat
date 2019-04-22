using KEKCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KEKCore.Contexts
{ 
    public class UsersDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MemeAsset> MemeOwners { get; set; }
        public DbSet<MemeEntry> MemeStash { get; set; }
        public DbSet<MarketplaceEntry> Marketplace { get; set; }

        public UsersDB()
            : base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}