using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using KEKChat.Contexts;
using KEKChat.Models;

namespace KEKChat.CoreAPI
{
    public static class Store
    {
        public static MemeModel GetStoreEntries()
        {
            using (UsersDB db = new UsersDB())
            {
                return new MemeModel(db.MemeStash
                                       .Where(meme => meme.VendorAmount > 0)
                                       .OrderBy(meme => meme.ID)
                                       .ToList());
            }
        }

        public static void BuyMeme(MemeModel meme, int memeID, string username)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (UsersDB db = new UsersDB())
                {
                    User user = db.Users
                                  .Where(u => u.Username == username)
                                  .SingleOrDefault();

                    decimal userCurrency = user.Currency;

                    MemeEntry currentMeme = db.MemeStash
                                       .Where(u => u.ID == memeID)
                                       .SingleOrDefault();

                    decimal memePrice = currentMeme.Price;

                    decimal totalPrice = memePrice * meme.Quantity;

                    if (userCurrency >= totalPrice && currentMeme.VendorAmount >= meme.Quantity && meme.Quantity > 0)
                    {

                        user.Currency -= totalPrice;
                        currentMeme.VendorAmount -= meme.Quantity;

                        MemeAsset asset = new MemeAsset(user, currentMeme, meme.Quantity, meme.AssetName);

                        var existingAsset = db.MemeOwners
                                              .Where(a => a.UserID == user.ID
                                                       && a.MemeID == memeID)
                                              .SingleOrDefault();

                        if (existingAsset == null)
                            db.MemeOwners.Add(asset);
                        else
                        {
                            existingAsset.Amount += meme.Quantity;
                        }

                        db.SaveChanges();
                    }
                }

                scope.Complete();
            }
        }
    }
}