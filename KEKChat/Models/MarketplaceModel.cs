using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KEKCore.Entities;

namespace KEKChat.Models
{

    public class MarketplaceModel
    {
        public MarketplaceEntry Meme { get; set; } = null;
        public int Quantity { get; set; }
        
        public MarketplaceModel() { }

        public MarketplaceModel(MarketplaceEntry meme)
        {
            Meme = meme;
        }
    }
}