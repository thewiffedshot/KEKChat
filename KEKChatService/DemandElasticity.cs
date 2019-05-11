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
        public static void ReevaluatePrices()
        {
            using (UsersDB db = new UsersDB())
            {
                var memes = db.MemeStash.Where(m => m.VendorAmount > 0).ToList();
                var transactions = db.Transactions.Where(t => t.SellerID == null).ToList();

                foreach (MemeEntry meme in memes)
                {
                    DateTime currentTime = DateTime.Now;

                    var previousTransactions = transactions.Where(t => (currentTime - t.TimeStamp).TotalMinutes < 4 &&
                                                                       (currentTime - t.TimeStamp).TotalMinutes >= 2 &&
                                                                        t.MemeID == meme.ID).ToList();

                    var recentTransactions = transactions.Where(t => (currentTime - t.TimeStamp).TotalMinutes < 2).ToList();

                    if (previousTransactions.Count != 0 && recentTransactions.Count != 0)
                    {
                        decimal currentPrice = recentTransactions.First().Value;

                        int currentDemand = recentTransactions.Sum(t => t.Quantity);
                        int previousDemand = previousTransactions.Sum(t => t.Quantity);

                        decimal deltaDemand = (currentDemand - previousDemand) / previousDemand;

                        decimal targetDeltaPrice = -deltaDemand;

                        decimal newPrice = currentPrice + currentPrice * targetDeltaPrice;

                        meme.Price = newPrice;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
