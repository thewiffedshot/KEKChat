using KEKCore.Contexts;
using KEKCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KEKCore
{
    public class Store
    {
        public UsersDB DBContext { get; private set; }

        public Store()
        {
            DBContext = new UsersDB();
        }

        public Store(UsersDB context)
        {
            DBContext = context;
        }

        public void BuyMeme(string memeAssetName, int memeQuantity, int memeID, string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    User user = db.Users
                        .Single(u => u.Username == username);

                    decimal userCurrency = user.Currency;

                    MemeEntry currentMeme = db.MemeStash
                        .Single(u => u.ID == memeID);

                    decimal memePrice = currentMeme.Price;

                    decimal totalPrice = memePrice * memeQuantity;

                    if (userCurrency >= totalPrice && currentMeme.VendorAmount >= memeQuantity && memeQuantity > 0)
                    {

                        user.Currency -= totalPrice;
                        currentMeme.VendorAmount -= memeQuantity;

                        MemeAsset asset = new MemeAsset
                                              {
                                                  UserID = user.ID,
                                                  MemeID = currentMeme.ID,
                                                  Amount = memeQuantity,
                                                  AssetName = string.IsNullOrEmpty(memeAssetName)
                                                                  ? "meme_" + currentMeme.ID
                                                                  : memeAssetName
                                              };

                        MemeAsset existingAsset = db.MemeOwners
                            .SingleOrDefault(
                                a => a.UserID == user.ID
                                     && a.MemeID == memeID);

                        db.Transactions.Add(
                            new Transaction
                                {
                                    BuyerID = user.ID,
                                    SellerID = null,
                                    SellerName = "STORE",
                                    Value = memePrice,
                                    AssetName = existingAsset == null ? asset.AssetName : existingAsset.AssetName,
                                    Quantity = memeQuantity,
                                    TimeStamp = DateTime.Now,
                                    MemeID = memeID
                                });

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

        public IEnumerable<MemeEntry> GetStoreEntries(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;
            

            List<MemeEntry> memes = db.MemeStash
                .ToList();

            List<OrderWeight> orderWeights = db.OrderWeights
                .Include(w => w.User)
                .Where(w => w.User.Username == username)
                .ToList();

            return OrderMemesByPreferences(memes, orderWeights);
        }

        private static List<MemeEntry> OrderMemesByPreferences(List<MemeEntry> memes, List<OrderWeight> orderWeights)
        {
            //List<decimal> weights = orderWeights.Select(u => u.Weight).ToList();

            return memes
                .OrderByDescending(meme => orderWeights.Single(w => w.MemeID == meme.ID).Weight)
                .ThenByDescending(meme => meme.GoldCount)
                .ThenByDescending(meme => meme.ID)
                .Where(meme => meme.VendorAmount > 0)
                .ToList();
        }
    }
}