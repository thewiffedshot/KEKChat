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

            const decimal PriceInterval = 100;

            var memes = db.MemeStash
                .Where(meme => meme.VendorAmount > 0)
                .ToList();

            List<Transaction> transactions = db.Transactions
                .Include(t => t.Buyer)
                .Where(t => t.Buyer.Username == username)
                .ToList();

            // If user has not bought memes, show newest first
            if (transactions.Count == 0)
                return memes.OrderByDescending(meme => meme.ID).ToList();

            // Else, show newest, ordered by most bought price category
            decimal highestPrice = memes.OrderBy(m => m.Price).Last().Price;

            List<decimal> sortingWeights = Enumerable
                .Repeat(0m, CalculatePriceRangeIndex(highestPrice, PriceInterval) + 1)
                .ToList();

            foreach (Transaction trans in transactions)
            {
                int index = CalculatePriceRangeIndex(trans.Value, PriceInterval);

                sortingWeights[index] += trans.Value * trans.Quantity;
            }

            return memes
                .OrderByDescending(meme => GetMemeWeight(meme.Price, PriceInterval, sortingWeights))
                .ThenByDescending(meme => meme.ID);
        }

        private static decimal GetMemeWeight(decimal memePrice, decimal priceInterval, List<decimal> sortingWeights)
        {
            return sortingWeights[CalculatePriceRangeIndex(memePrice, priceInterval)];
        }

        private static int CalculatePriceRangeIndex(decimal price, decimal interval)
        {
            return (int)Math.Floor(price / interval);
        }
    }
}