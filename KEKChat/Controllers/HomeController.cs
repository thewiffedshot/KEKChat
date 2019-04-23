using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using KEKCore;

namespace KEKChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            UpdateUserCurrencyLabel();

            return View("Chat");
        }

        public ActionResult Home()
        {
            // TODO: Make this a tester method.

            return RedirectToAction("Chat");
        }

        public ActionResult Store(MemeModel meme)
        {
            return View();
        }

        public ActionResult StoreInit()
        {
            UpdateUserCurrencyLabel();

            return View("Store", KEKCore.Store.GetStoreEntries().Select(meme => new MemeModel(meme)));
        }
        
        [HttpPost]
        public ActionResult Heartbeat()
        {
            KEKCore.Account.SendHeartbeat(User.Identity.Name);

            return null;
        }

        [HttpPost]
        public ActionResult SendMessage(string msg)
        {
            KEKCore.Chat.SendMessage(msg, User.Identity.Name);

            return null;
        }

        [HttpPost]
        public ActionResult SendMeme(int memeID)
        {
            KEKCore.Chat.SendMeme(memeID, User.Identity.Name);

            return null;
        }

        public ActionResult GetMessages(int lastMessageID)
        {
            return PartialView("_ChatView", 
                KEKCore.Chat.GetMessages(lastMessageID)
                            .Select(m =>
                                new MessageModel
                                {
                                    ID = m.ID,
                                    Username = m.Username,
                                    Date = m.Date,
                                    ImageSource = m.Meme != null ? m.Meme.ImagePath : null,
                                    Text = m.Text
                                }));
        }

        public ActionResult GetPeople()
        {
            return PartialView("_PeopleList", KEKCore.Session.GetPeopleList()
                                                             .Select(u => new PeopleModel(u.Username, u.LastOnline))
                                                             .OrderByDescending(u => u.Online)
                                                             .ThenBy(u => u.Username));
        }

        public ActionResult GetInventory(string view)
        {
            var list = KEKCore.Session.GetInventoryList(User.Identity.Name);
            
            switch (view)
            {
                case "Chat":
                    return PartialView("~/Views/Home/Inventory/_ChatInventoryView.cshtml", list.Select(i => new InventoryModel(i)));
                case "Store":
                    return PartialView("~/Views/Home/Inventory/_StoreInventoryView.cshtml", list.Select(i => new InventoryModel(i)));
                case "Marketplace":
                    return PartialView("~/Views/Home/Inventory/_MarketplaceInventoryView.cshtml", list.Select(i => new MarketplaceInventoryModel(i)));
                default:
                    return null;
            }
        }

        [HttpPost]
        public ActionResult BuyMeme(MemeModel meme, int buy)
        {
            if (ModelState.IsValid)
            {
                KEKCore.Store.BuyMeme(meme.AssetName, meme.Quantity, buy, User.Identity.Name);
            }

            return StoreInit();
        }

        [HttpPost]
        public ActionResult SellMeme(MarketplaceInventoryModel meme, int sell)
        {
            if (ModelState.IsValid)
            {
                KEKCore.Marketplace.SellMeme(meme.Quantity, meme.Price, sell, User.Identity.Name);
            }

            return Marketplace();
        }

        public ActionResult Marketplace()
        {
            UpdateUserCurrencyLabel();

            return View("Marketplace", KEKCore.Marketplace.GetMarketplaceEntries()
                                                          .Select(u => new MarketplaceModel(u)));
        }

        public ActionResult TradeMeme(MarketplaceModel meme, int buy)
        {
            if (ModelState.IsValid)
            {
                KEKCore.Marketplace.TradeMeme(meme.Quantity, buy, User.Identity.Name);
            }

            return StoreInit();
        }

        public void UpdateUserCurrencyLabel()
        {
            Session["currency"] = KEKCore.Session.GetUserCurrency(User.Identity.Name);
        }
    }
}