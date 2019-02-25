namespace KEKChat.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
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
        DbSet<LoginModel> Users { get; set; }

        public UsersDB()
            :base("name=UsersDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().Property(u => u.Username).HasColumnName("username");
            modelBuilder.Entity<LoginModel>().Property(u => u.Password).HasColumnName("password");
        }
    }
}