namespace KEKChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public class LoginModel
    {
        [Required, Key]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UsersDB : DbContext
    {
        public DbSet<LoginModel> Users { get; set; }

        public UsersDB()
            :base("name=UsersDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UsersDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<LoginModel>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }
    }
}