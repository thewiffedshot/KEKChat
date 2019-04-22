using KEKChat.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Entities;

namespace KEKChat.Models
{
    public class InventoryModel
    {
        public IEnumerable<MemeAsset> InventoryList { get; set; } = new List<MemeAsset>(0);

        public InventoryModel()
        {

        }

        public InventoryModel(IEnumerable<MemeAsset> list)
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