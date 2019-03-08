using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    [Table("inventory")]
    public class Meme
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public MemeInventory MemeInventory { get; set; }

        [ForeignKey("MemeInventory")]
        public int MemeID { get; set; }

        public int Amount { get; set; }

        public Meme(User user, MemeInventory meme, int amount)
        {

        }
    }
}