using System.Data.Entity;

using KEKCore.Entities;

namespace KEKCore.Contexts
{ 
    public class UsersDB : DbContext
    {

        public UsersDB()
            : base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersDB>());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MemeAsset> MemeOwners { get; set; }

        public DbSet<MemeEntry> MemeStash { get; set; }

        public DbSet<MarketplaceEntry> Marketplace { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<OrderWeight> OrderWeights { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<MemeEntry>().HasIndex(u => u.ImagePath).IsUnique();
            modelBuilder.Entity<OrderWeight>().HasIndex(u => new { u.MemeID, u.UserID }).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}