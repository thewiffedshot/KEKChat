using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KEKCore.Entities
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

        public DateTime LastOnline { get; set; }
    }
}