using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class MemeModel
    {
        public List<MemeEntry> Memes { get; set; }

        public int Quantity { get; set; }

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