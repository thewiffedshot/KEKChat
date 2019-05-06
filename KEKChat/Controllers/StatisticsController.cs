using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace KEKChat.Controllers
{
    public class StatisticsController : Controller
    {
        public ActionResult Dashboard()
        {
            return View(KEKCore.Account.GetTransactions(User.Identity.Name)
                                       .Select(t => new TransactionModel {
                                           ID = t.ID,
                                           BuyerName = t.Buyer.Username,
                                           SellerName = t.Seller == null ? "Store" : t.Seller.Username,
                                           Value = t.Value,
                                           Quantity = t.Quantity,
                                           AssetName = t.AssetName,
                                           TimeStamp = t.TimeStamp
                                       }));
        }
    }
}