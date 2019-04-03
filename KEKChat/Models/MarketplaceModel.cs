namespace KEKChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Contexts;

    public class MarketplaceModel
    {
        public List<MarketplaceEntry> MemesForSale { get; set; } = new List<MarketplaceEntry>(0);
        public int Quantity { get; set; }
        
        public MarketplaceModel() { }

        public MarketplaceModel(List<MarketplaceEntry> memes)
        {
            MemesForSale = memes;
        }
    }
}