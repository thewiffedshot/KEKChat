using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
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

                        if (currentMeme.Amount >= memeQuantity && memeQuantity > 0)
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
                                db.Marketplace.Add(new MarketplaceEntry { AssetID = currentMeme.ID,
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

        public static IEnumerable<MarketplaceEntry> GetMarketplaceEntries()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Marketplace.Include(u => u.MemeAsset.MemeEntry).ToList();
            }
        }

        public static void TradeMeme(int quantity, int marketEntryID, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    //try
                    {
                        MarketplaceEntry marketEntry = db.Marketplace.Where(m => m.ID == marketEntryID)
                                                                     .Include(m => m.MemeAsset.MemeEntry)
                                                                     .Include(m => m.User)
                                                                     .SingleOrDefault();

                        User buyer = db.Users
                                      .Where(u => u.Username == username)
                                      .SingleOrDefault();

                        User owner = marketEntry.User;

                        decimal memePrice = marketEntry.Price;

                        decimal totalPrice = memePrice * quantity;

                        if (buyer.Currency >= totalPrice && marketEntry.Quantity >= quantity && quantity > 0)
                        {

                            buyer.Currency -= totalPrice;
                            owner.Currency += totalPrice;

                            marketEntry.Quantity -= quantity;

                            MemeAsset asset = new MemeAsset { UserID = buyer.ID,
                                                              MemeID = marketEntry.MemeAsset.MemeEntry.ID,
                                                              Amount = quantity,
                                                              AssetName = marketEntry.MemeAsset.AssetName

                            };

                            var existingAsset = db.MemeOwners
                                                  .Where(a => a.UserID == buyer.ID
                                                           && a.MemeID == marketEntry.MemeAsset.ID)
                                                  .SingleOrDefault();

                            if (existingAsset == null)
                                db.MemeOwners.Add(asset);
                            else
                            {
                                existingAsset.Amount += quantity;
                            }

                            db.SaveChanges();
                            trans.Commit();
                        }
                    }
                    /*catch (Exception e)
                    {
                        trans.Rollback();
                    }*/
                }
            }
        }
    }
}