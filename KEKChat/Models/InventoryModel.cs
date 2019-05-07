using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Entities;

namespace KEKChat.Models
{
    public class InventoryModel
    {
        public MemeAsset Meme { get; set; }

        public InventoryModel()
        {

        }

        public InventoryModel(MemeAsset meme)
        {
            Meme = meme;
        }
    }
}