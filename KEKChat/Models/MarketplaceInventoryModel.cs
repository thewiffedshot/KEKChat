using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KEKCore.Entities;
using KEKCore.Contexts;

namespace KEKChat.Models
{
    public class MarketplaceInventoryModel
    {
        public MemeAsset Meme { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public MarketplaceInventoryModel()
        {

        }

        public MarketplaceInventoryModel(MemeAsset meme)
        {
            Meme = meme;
        }
    }
}