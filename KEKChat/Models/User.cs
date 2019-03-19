using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string HashSalt { get; set; }

        public string HashIterations { get; set; }

        public decimal Currency { get; set; } = 5000;

        public bool IsOnline { get; set; } = false;

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
}