using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using KEKCore;
using KEKCore.Contexts;

namespace KEKChat.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private static UsersDB db = new UsersDB();

        private readonly KEKCore.Store store = new KEKCore.Store(db);
        private readonly KEKCore.Account account = new KEKCore.Account(db);
        private readonly KEKCore.Chat chat = new KEKCore.Chat(db);
        private readonly KEKCore.Session session = new KEKCore.Session(db);
        private readonly KEKCore.Marketplace marketplace = new KEKCore.Marketplace(db);


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

            return View("Store", store.GetStoreEntries(User.Identity.Name).Select(meme => new MemeModel(meme)));
        }
        
        [HttpPost]
        public ActionResult Heartbeat()
        {
            account.SendHeartbeat(User.Identity.Name);

            return null;
        }

        [HttpPost]
        public ActionResult SendMessage(string msg)
        {
            chat.SendMessage(msg, User.Identity.Name);

            return null;
        }

        [HttpPost]
        public ActionResult SendMeme(int memeID)
        {
            chat.SendMeme(memeID, User.Identity.Name);

            return null;
        }

        public ActionResult GetMessages(int lastMessageID)
        {
            return PartialView("_ChatView",
                chat.GetMessages(lastMessageID)
                            .Select(m =>
                                new MessageModel
                                {
                                    ID = m.ID,
                                    Username = m.User.Username,
                                    Date = m.Date,
                                    ImageSource = m.Meme?.ImagePath,
                                    Text = m.Text
                                }));
        }

        public ActionResult GetPeople()
        {
            return PartialView("_PeopleList",
                session.GetPeopleList().Select(u => new PeopleModel(u.Username, u.LastOnline)).OrderByDescending(u => u.Online)
                                                             .ThenBy(u => u.Username));
        }

        public ActionResult GetInventory(string view)
        {
            var list = session.GetInventoryList(User.Identity.Name);
            
            switch (view)
            {
                case "Chat":
                    return PartialView("~/Views/Home/Inventory/_ChatInventoryView.cshtml", list.Select(i => new InventoryModel(i)));
                case "Store":
                    return PartialView("~/Views/Home/Inventory/_StoreInventoryView.cshtml", list.Select(i => new InventoryModel(i)));
                case "Dashboard":
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
                store.BuyMeme(meme.AssetName, meme.Quantity, buy, User.Identity.Name);
            }

            return StoreInit();
        }

        [HttpPost]
        public ActionResult SellMeme(MarketplaceInventoryModel meme, int sell)
        {
            if (ModelState.IsValid)
            {
                marketplace.SellMeme(meme.Quantity, meme.Price, sell, User.Identity.Name);
            }

            return Marketplace();
        }

        public ActionResult Marketplace()
        {
            UpdateUserCurrencyLabel();

            return View("Marketplace", marketplace.GetMarketplaceEntries().Select(u => new MarketplaceModel(u)));
        }

        public ActionResult TradeMeme(MarketplaceModel meme, int buy)
        {
            if (ModelState.IsValid)
            {
                marketplace.TradeMeme(meme.Quantity, buy, User.Identity.Name);
            }

            return StoreInit();
        }

        public void UpdateUserCurrencyLabel()
        {
            Session["currency"] = session.GetUserCurrency(User.Identity.Name);
        }
    }
}