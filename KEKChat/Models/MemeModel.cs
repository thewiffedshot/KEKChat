using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using KEKCore.Entities;

namespace KEKChat.Models
{
    public class MemeModel
    {
        public MemeEntry Meme { get; set; } = null;

        [Required]
        public int Quantity { get; set; }

        [MaxLength(64, ErrorMessage = "Meme name too long. (max 64)")]
        public string AssetName { get; set; }

        public int MemeID { get; set; }

        public MemeModel(MemeEntry meme)
        {
            Meme = meme;
            Quantity = 1;
        }

        public MemeModel()
        {
            Quantity = 1;
        }
    }
}