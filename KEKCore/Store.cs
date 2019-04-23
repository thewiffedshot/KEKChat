using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Contexts;
using KEKCore.Entities;
using System.Data.Entity;

namespace KEKCore
{
    public static class Store
    {
        public static IEnumerable<MemeEntry> GetStoreEntries()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.MemeStash
                         .Where(meme => meme.VendorAmount > 0)
                         .OrderBy(meme => meme.ID)
                         .ToList();
            }
        }

        public static void BuyMeme(string memeAssetName, int memeQuantity, int memeID, string username)
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

                        decimal userCurrency = user.Currency;

                        MemeEntry currentMeme = db.MemeStash
                                           .Where(u => u.ID == memeID)
                                           .SingleOrDefault();

                        decimal memePrice = currentMeme.Price;

                        decimal totalPrice = memePrice * memeQuantity;

                        if (userCurrency >= totalPrice && currentMeme.VendorAmount >= memeQuantity && memeQuantity > 0)
                        {

                            user.Currency -= totalPrice;
                            currentMeme.VendorAmount -= memeQuantity;

                            MemeAsset asset = new MemeAsset { UserID = user.ID,
                                                              MemeID = currentMeme.ID,
                                                              Amount = memeQuantity,
                                                              AssetName = string.IsNullOrEmpty(memeAssetName) ? "meme_" + currentMeme.ID : memeAssetName
                            };

                            var existingAsset = db.MemeOwners
                                                  .Where(a => a.UserID == user.ID
                                                           && a.MemeID == memeID)
                                                  .SingleOrDefault();

                            if (existingAsset == null)
                                db.MemeOwners.Add(asset);
                            else
                            {
                                existingAsset.Amount += memeQuantity;
                            }

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
    }
}