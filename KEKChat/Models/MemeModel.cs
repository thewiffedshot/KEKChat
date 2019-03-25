using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KEKChat.Models
{
    public class MemeModel
    {
        public List<MemeEntry> Memes { get; set; } = new List<MemeEntry>(0);

        [Required]
        public int Quantity { get; set; }

        [MaxLength(64, ErrorMessage = "Meme name too long. (max 64)")]
        public string AssetName { get; set; }

        public int MemeID { get; set; }

        public MemeModel(List<MemeEntry> collection)
        {
            Memes = collection;
        }

        public MemeModel()
        {

        }
    }
}