namespace KEKChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using KEKChat.Contexts;

    public class MarketplaceInventoryModel
    {

        public List<MemeAsset> InventoryList { get; set; } = new List<MemeAsset>(0);
        public int Quantity { get; set; } = 1;
        public int Price { get; set; } = 1;

        public MarketplaceInventoryModel()
        {

        }

        public MarketplaceInventoryModel(List<MemeAsset> list)
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