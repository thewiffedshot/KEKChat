using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public User User { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public string Username { get; set; }

        public Message(string message, User user)
        {
            Text = message;
            User = user;
            UserID = user.ID;
            Username = user.Username;
        }

        public Message() { }
    }
}