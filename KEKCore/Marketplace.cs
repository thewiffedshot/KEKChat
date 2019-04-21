using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKCore
{
    public static class Marketplace
    {
        public static void SellMeme(int memeQuantity, decimal memePrice, int assetID, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        User user = db.Users
                                      .Where(u => u.Username == username)
                                      .SingleOrDefault();

                        MemeAsset currentMeme = db.MemeOwners
                                           .Where(u => u.ID == assetID && u.UserID == user.ID)
                                           .SingleOrDefault();

                        if (currentMeme.Amount >= memeQuantity)
                        {
                            currentMeme.Amount -= memeQuantity;

                            var existingMemeForSale = db.Marketplace
                                                      .Where(a => a.SellerID == user.ID
                                                               && a.AssetID == assetID
                                                               && a.Price == memePrice)
                                                      .SingleOrDefault();

                            if (existingMemeForSale != null)
                                existingMemeForSale.Quantity += memeQuantity;
                            else
                                db.Marketplace.Add(new MarketplaceEntry(currentMeme, user, memeQuantity, memePrice));

                            db.SaveChanges();
                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        public static IEnumerable<MarketplaceEntry> GetMarketplaceEntries()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Marketplace.ToList();
            }
        }
    }
}