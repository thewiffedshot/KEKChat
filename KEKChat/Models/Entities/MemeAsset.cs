using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    [Table("memeAssets")]
    public class MemeAsset
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

        public string AssetName { get; set; } = null;

        public MemeAsset(User user, MemeEntry meme, int amount, string name)
        {
            if (name == null || name == "")
                name = "meme_" + meme.ID;

            User = user;

            UserID = User.ID;

            MemeEntry = meme;

            MemeID = meme.ID;

            Amount = amount;

            AssetName = name;
        }

        public MemeAsset()
        {

        }
    }
}