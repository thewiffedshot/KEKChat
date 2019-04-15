using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using KEKChat.Contexts;
using KEKChat.Models;

namespace KEKChat.CoreAPI
{
    public static class Marketplace
    {
        public static void SellMeme(MarketplaceInventoryModel meme, int assetID, string username)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (UsersDB db = new UsersDB())
                {
                    User user = db.Users
                                  .Where(u => u.Username == username)
                                  .SingleOrDefault();

                    MemeAsset currentMeme = db.MemeOwners
                                       .Where(u => u.ID == assetID && u.UserID == user.ID)
                                       .SingleOrDefault();

                    if (currentMeme.Amount >= meme.Quantity)
                    {
                        currentMeme.Amount -= meme.Quantity;

                        var existingMemeForSale = db.Marketplace
                                                  .Where(a => a.SellerID == user.ID
                                                           && a.AssetID == assetID
                                                           && a.Price == meme.Price)
                                                  .SingleOrDefault();

                        if (existingMemeForSale != null)
                            existingMemeForSale.Quantity += meme.Quantity;
                        else
                            db.Marketplace.Add(new MarketplaceEntry(currentMeme, user, meme.Quantity, meme.Price));

                        db.SaveChanges();
                    }
                }
            }
        }

        public static MarketplaceModel GetMarketplaceModel()
        {
            using (UsersDB db = new UsersDB())
            {
                return new MarketplaceModel(db.Marketplace.ToList());
            }
        }
    }
}