using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    [Table("memeOwners")]
    public class MemeOwner
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public MemeEntry MemeEntry { get; set; }

        [ForeignKey("MemeEntry")]
        public int MemeID { get; set; }

        public int Amount { get; set; }

        public MemeOwner(User user, MemeEntry meme, int amount)
        {
            User = user;

            UserID = User.ID;

            MemeEntry = meme;

            MemeID = meme.ID;

            Amount = amount;
        }
    }
}