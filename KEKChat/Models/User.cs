﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string HashSalt { get; set; }

        public string HashIterations { get; set; }

        public decimal Currency { get; set; } = 50;

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