using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKCore
{


    public class Marketplace
    {
        public UsersDB DBContext { get; private set; }

        public Marketplace()
        {
            DBContext = new UsersDB();
        }

        public Marketplace(UsersDB context)
        {
            DBContext = context;
        }

        public void SellMeme(int memeQuantity, decimal memePrice, int assetID, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        User user = db.Users
                            .Single(u => u.Username == username);

                        MemeAsset currentMeme = db.MemeOwners
                            .Single(u => u.ID == assetID && u.UserID == user.ID);

                        if (currentMeme.Amount >= memeQuantity && memeQuantity > 0)
                        {
                            currentMeme.Amount -= memeQuantity;

                            MarketplaceEntry existingMemeForSale = db.Marketplace
                                .SingleOrDefault(
                                    a => a.SellerID == user.ID
                                         && a.AssetID == assetID
                                         && a.Price == memePrice);

                            if (existingMemeForSale != null)
                                existingMemeForSale.Quantity += memeQuantity;
                            else
                                db.Marketplace.Add(
                                    new MarketplaceEntry
                                    {
                                        AssetID = currentMeme.ID,
                                        SellerID = user.ID,
                                        Quantity = memeQuantity,
                                        Price = memePrice
                                    });

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

        public IEnumerable<MarketplaceEntry> GetMarketplaceEntries()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Marketplace.Include(u => u.MemeAsset.MemeEntry).ToList();
            }
        }

        public void TradeMeme(int quantity, int marketEntryId, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        MarketplaceEntry marketEntry = db.Marketplace.Where(m => m.ID == marketEntryId)
                            .Include(m => m.MemeAsset.MemeEntry)
                            .Include(m => m.User)
                            .Single();

                        User buyer = db.Users
                            .Single(u => u.Username == username);

                        User owner = marketEntry.User;

                        decimal memePrice = marketEntry.Price;

                        decimal totalPrice = memePrice * quantity;

                        if (buyer.Currency >= totalPrice && marketEntry.Quantity >= quantity && quantity > 0)
                        {

                            buyer.Currency -= totalPrice;
                            owner.Currency += totalPrice;

                            marketEntry.Quantity -= quantity;

                            MemeAsset asset = new MemeAsset
                            {
                                UserID = buyer.ID,
                                MemeID = marketEntry.MemeAsset.MemeEntry.ID,
                                Amount = quantity,
                                AssetName = marketEntry.MemeAsset.AssetName

                            };

                            MemeAsset existingAsset = db.MemeOwners
                                .SingleOrDefault(
                                    a => a.UserID == buyer.ID
                                         && a.MemeID == marketEntry.MemeAsset.ID);

                            db.Transactions.Add(
                                new Transaction
                                {
                                    BuyerID = buyer.ID,
                                    SellerID = owner.ID,
                                    SellerName = owner.Username,
                                    Value = memePrice,
                                    AssetName = existingAsset == null ? asset.AssetName : existingAsset.AssetName,
                                    Quantity = quantity,
                                    TimeStamp = DateTime.Now,
                                    MemeID = marketEntry.MemeAsset.MemeEntry.ID
                                });

                            if (existingAsset == null)
                                db.MemeOwners.Add(asset);
                            else
                            {
                                existingAsset.Amount += quantity;
                            }

                            if (marketEntry.Quantity == 0)
                                db.Marketplace.Remove(marketEntry);

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