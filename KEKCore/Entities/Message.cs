using KEKCore.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKCore.Entities
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

        public MemeEntry Meme { get; set; }
        
        [ForeignKey("Meme")]
        public int? MemeID { get; set; }

        public string Username { get; set; }

        public Message(string message, User user)
        {
            Text = message;
            User = user;
            UserID = user.ID;
            Username = user.Username;
            MemeID = null;
        }

        public Message(int img, User user)
        {
            MemeID = img;

            using (UsersDB db = new UsersDB())
            {
                Meme = db.MemeStash.Where(m => m.ID == MemeID).SingleOrDefault();
            }

            User = user;
            UserID = user.ID;
            Username = user.Username;
        }

        public Message() { }
    }
}