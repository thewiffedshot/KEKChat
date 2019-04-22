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
        public IEnumerable<MemeAsset> InventoryList { get; set; } = new List<MemeAsset>(0);
        public int Quantity { get; set; } = 1;
        public int Price { get; set; } = 1;

        public MarketplaceInventoryModel()
        {

        }

        public MarketplaceInventoryModel(IEnumerable<MemeAsset> list)
        {
            using (UsersDB db = new UsersDB())
            {
                foreach (var asset in list)
                    asset.MemeEntry = db.MemeStash
                                        .Where(m => m.ID == asset.MemeID)
                                        .SingleOrDefault();
            }

            InventoryList = list;
        }
    }
}