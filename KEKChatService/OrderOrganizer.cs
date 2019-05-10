using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKChatService
{
    public static class OrderOrganizer
    {
        public static void Organize()
        {
            using (UsersDB db = new UsersDB())
            {
                List<User> users = db.Users.ToList();

                List<MemeEntry> memes = db.MemeStash
                    .Where(meme => meme.VendorAmount > 0)
                    .ToList();

                foreach (User user in users)
                {
                    List<Transaction> transactions = db.Transactions
                        .Include(t => t.Buyer)
                        .Where(t => t.Buyer.Username == user.Username)
                        .ToList();

                    const decimal PriceInterval = 100;

                    if (transactions.Count == 0)
                        foreach (MemeEntry meme in memes)
                        {
                            OrderWeight weight = db.OrderWeights.SingleOrDefault(w => w.UserID == user.ID && w.MemeID == meme.ID);
                            if (weight == null)
                            {
                                db.OrderWeights.Add(
                                    new OrderWeight() { UserID = user.ID, MemeID = meme.ID, Weight = 0 });
                            }
                        }
                    else
                    {
                        decimal highestPrice = memes.OrderBy(m => m.Price).Last().Price;

                        List<decimal> priceWeights = Enumerable
                            .Repeat(0m, CalculatePriceRangeIndex(highestPrice, PriceInterval) + 1)
                            .ToList();

                        foreach (Transaction trans in transactions)
                        {
                            int index = CalculatePriceRangeIndex(trans.Value, PriceInterval);

                            priceWeights[index] += trans.Value * trans.Quantity;
                        }

                        List<decimal> memeWeights = new List<decimal>();

                        foreach (MemeEntry meme in memes)
                        {
                            decimal subredditRatio = memes.Count / memes.Count(u => u.Subreddit == meme.Subreddit);

                            decimal nsfwRatio = memes.Count / memes.Count(u => u.NSFW == meme.NSFW);

                            decimal memeWeight = GetPriceWeight(meme.Price, PriceInterval, priceWeights) * subredditRatio * nsfwRatio;

                            OrderWeight weight = db.OrderWeights.SingleOrDefault(w => w.UserID == user.ID && w.MemeID == meme.ID);
                            if (weight == null)
                                db.OrderWeights.Add(
                                    new OrderWeight() { UserID = user.ID, MemeID = meme.ID, Weight = memeWeight });
                            else
                                weight.Weight = memeWeight;
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        private static decimal GetPriceWeight(decimal memePrice, decimal priceInterval, List<decimal> priceWeights)
        {
            return priceWeights[CalculatePriceRangeIndex(memePrice, priceInterval)];
        }

        private static int CalculatePriceRangeIndex(decimal price, decimal interval)
        {
            return (int)Math.Floor(price / interval);
        }
    }
}
