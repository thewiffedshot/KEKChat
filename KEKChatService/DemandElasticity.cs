using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKChatService
{
    public static class DemandElasticity
    {
        public static void ReevaluatePrices(int minutes)
        {
            using (UsersDB db = new UsersDB())
            {
                var memes = db.MemeStash.Where(m => m.VendorAmount > 0).ToList();
                var transactions = db.Transactions.Where(t => t.SellerID == null).ToList();

                foreach (MemeEntry meme in memes)
                {
                    DateTime currentTime = DateTime.Now;

                    var previousTransactions = transactions.Where(t => (currentTime - t.TimeStamp).TotalMinutes < 2 * minutes &&
                                                                       (currentTime - t.TimeStamp).TotalMinutes >= minutes &&
                                                                        t.MemeID == meme.ID).ToList();

                    var recentTransactions = transactions.Where(t => (currentTime - t.TimeStamp).TotalMinutes < minutes &&
                                                                     t.MemeID == meme.ID).ToList();

                    //Another algorithm we tried, but we didn't understand thoroughly
                    /*if (previousTransactions.Count != 0 && recentTransactions.Count != 0)
                    {
                        decimal currentPrice = recentTransactions.First().Value;
                        decimal previousPrice = previousTransactions.First().Value;

                        int currentDemand = recentTransactions.Sum(t => t.Quantity);
                        int previousDemand = previousTransactions.Sum(t => t.Quantity);

                        if (previousDemand == currentDemand || previousPrice == currentPrice)
                            continue;

                        decimal slope = (previousPrice - currentPrice) / (previousDemand - currentDemand);

                        decimal constant = slope * previousDemand + previousPrice;

                        int quantity = (int)Math.Ceiling(constant / (2 * slope));

                        decimal optimalPrice = constant - slope * quantity;

                        meme.Price = optimalPrice;
                    }
                }*/

                    int currentDemand = recentTransactions.Count == 0 ? 0 : recentTransactions.Sum(t => t.Quantity);
                    int previousDemand = previousTransactions.Count == 0 ? 0 : previousTransactions.Sum(t => t.Quantity);

                    int deltaDemand = currentDemand - previousDemand;
                    decimal deltaDemandPercent = (decimal)deltaDemand / meme.InitCount;

                    decimal priceMult = 1 + deltaDemandPercent;
                    meme.Price *= priceMult;

                    if (previousDemand + currentDemand == 0)
                    {
                        // Scale priceMult by service check interval(avg price change: 0.8 per day);
                        meme.Price *= 1 - 0.2m * minutes / 1440;
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}
