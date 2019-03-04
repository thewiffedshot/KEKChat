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
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password confirmation invalid. Please confirm password again."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class User
    {
        [Key]
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string HashSalt { get; set; }

        public string HashIterations { get; set; }

        public User(string name, string passhash, string salt, string iterations)
        {
            Username = name;
            PasswordHash = passhash;
            HashSalt = salt;
            HashIterations = iterations;
        }

        public User()
        {

        }
    }

    public class UsersDB : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDB()
            :base("name=UsersDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UsersDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<User>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }
    }
}