using KEKChat.Contexts;
using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using KEKChat.CoreAPI;

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

            return View("Store", CoreAPI.Store.GetStoreEntries());
        }
        
        [HttpPost]
        public ActionResult Heartbeat()
        {
            CoreAPI.Account.SendHeartbeat(User.Identity.Name);

            return null;
        }

        [HttpPost]
        public ActionResult SendMessage(string msg, int lastMessageID)
        {
            CoreAPI.Chat.SendMessage(msg, User.Identity.Name);

            return GetMessages(lastMessageID);
        }

        [HttpPost]
        public ActionResult SendMeme(string memeID, int lastMessageID)
        {
            CoreAPI.Chat.SendMeme(memeID, User.Identity.Name);

            return GetMessages(lastMessageID);
        }

        public ActionResult GetMessages(int lastMessageID)
        {
            return PartialView("_ChatView", CoreAPI.Chat.GetMessages(lastMessageID));
        }

        public ActionResult GetPeople()
        {
            return PartialView("_PeopleList", CoreAPI.Session.GetPeopleListModel());
        }

        public ActionResult GetInventory(string view)
        {
            List<MemeAsset> list = CoreAPI.Session.GetInventoryList(User.Identity.Name);
            
            switch (view)
            {
                case "Chat":
                    return PartialView("~/Views/Home/Inventory/_ChatInventoryView.cshtml", new InventoryModel(list));
                case "Store":
                    return PartialView("~/Views/Home/Inventory/_StoreInventoryView.cshtml", new InventoryModel(list));
                case "Marketplace":
                    return PartialView("~/Views/Home/Inventory/_MarketplaceInventoryView.cshtml", new MarketplaceInventoryModel(list));
                default:
                    return null;
            }
        }

        [HttpPost]
        public ActionResult BuyMeme(MemeModel meme, int buy)
        {
            if (ModelState.IsValid)
            {
                CoreAPI.Store.BuyMeme(meme, buy, User.Identity.Name);
            }

            return StoreInit();
        }

        [HttpPost]
        public ActionResult SellMeme(MarketplaceInventoryModel meme, int sell)
        {
            if (ModelState.IsValid)
            {
                CoreAPI.Marketplace.SellMeme(meme, sell, User.Identity.Name);
            }

            return Marketplace();
        }

        public ActionResult Marketplace()
        {
            UpdateUserCurrencyLabel();

            return View("Marketplace", CoreAPI.Marketplace.GetMarketplaceModel());
        }

        public void UpdateUserCurrencyLabel()
        {
            Session["currency"] = CoreAPI.Session.GetUserCurrency(User.Identity.Name);
        }
    }
}