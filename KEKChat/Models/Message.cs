using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public User User { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }

        public Message(string message, User user)
        {
            Text = message;
            User = user;
            Username = user.Username;
        }

        public Message() { }
    }
}